import toastr from 'toastr';

let grids = [];

export class ELApp {
    constructor() {
        this.culture = 'en';
        this.user = '';
    }

    showNotification(msg) {
        switch (msg.Type) {
            case 1:
                toastr.success(msg.Message, msg.Title);
                break;
            case 2:
                toastr.error(msg.Message, msg.Title);
                break;
            case 3:
                toastr.warning(msg.Message, msg.Title);
                break;
            default:
                toastr.info(msg.Message, msg.Title);
                break;
        }
    }

    addReturnUrl(href) {
        let urlManager = new SCIUrlManager(href);
        urlManager.addParam('returnUrl', window.location.pathname + window.location.search);
        window.location = urlManager.combine();

        return false;
    }

    loadTotalLeaveRequests() {
        if(!this.user) {
            return;
        }

        $.ajax({
            url: '/LeaveRequests/TotalLeaveRequests',
            type: 'GET',
            dataType: 'json',
            success: function(total) {
                $('.leave-requests-badge').html(total);
                if(total == 0) {
                    $('.leave-requests-badge').hide();
                }
                else {
                    $('.leave-requests-badge').show();
                }
            }
        });
    }
}

export class SCIUrlManager {
    constructor(url) {
        let pieces = url.split('?');
        
        this.url = pieces[0] || '';
        this.params = {};
        
        let params = (pieces[1] || '').split('&');
        for(let i = 0; i < params.length; i++) {
            if(!params[i]) {
                continue;
            }

            pieces = params[i].split('=');
            if (pieces.length > 1) {
                this.params[pieces[0]] = pieces[1];
            }
        }
    };

    combine() {
        if(Object.keys(this.params).length === 0) {
            return this.url;
        }

        let params = [];
        for(let key in this.params) {
            params.push(key + '=' + this.params[key]);
        }

        return this.url + '?' + params.join('&');
    };

    addParam(key, value) {
        this.params[key] = encodeURIComponent(value);
    }
};

export class SCIAjaxGrid {
    constructor(grid) {
        let $this = this;

        $this.grid = grid;
        $this.url = grid.attr('data-url');
        $this.currentUrl = grid.data('current-url');
        $this.useHistory = grid.attr('data-use-history') !== undefined && window.history.pushState;

        grid.on('click', 'thead a:not(.disabled), .pagination a:not(.disabled)', function (e) {
            e.preventDefault();
            $this.load($(this).attr('href'), true);
        });

        grid.on('sci-ajax-grid-load', function () {
            $this.load($this.grid.data('current-url'));
        });

        grid.parents('.sci-ajax-grid-wrapper').first().find('.sci-ajax-grid-search-form').submit(function(e) {
            let searchTerm = $(this).find('[name="searchTerm"]').val();
            let url = $this.url;

            let urlManager = new SCIUrlManager(url);
            urlManager.addParam('searchTerm', searchTerm);

            $this.load(urlManager.combine(), true);
        });

        if (grid.hasClass('sci-ajax-grid-auto')) {
            $this.load($this.url, false);
        }

        if ($this.useHistory) {
            window.addEventListener('popstate', function (e) {
                if (e.state) {
                    $this.load(e.state, false);
                }
                else {
                    window.location = document.location;
                }
            });
        }
    };

    addHistory(url) {
        if (this.useHistory) {
            window.history.pushState(url, document.title, url);
        }
    };

    load(url, useHistory) {
        let $this = this;
        let grid = $this.grid;
        $this.currentUrl = url;
        grid.data('current-url', url);

        $.ajax({
            url: url,
            type: 'GET',
            cache: false,
            success: function (html) {
                grid.html(html);

                grid.find('form').each(function () {
                    $.validator.unobtrusive.parse($(this));
                    if ($.fn.datepicker) {
                        $(this).find('.sci-date-picker').datepicker({ format: 'dd.mm.yyyy' });
                    }
                });

                if (useHistory) {
                    $this.addHistory(url);
                }
            }
        });
    };
};

export class SCIForm {
    static onAjaxBegin() {
        $(this).find(':submit').prop('disabled', true);
    };

    static onAjaxSuccess(data) {
        let form = $(this);
        form.find(':submit').prop('disabled', false);

        let errorList = form.find('.alert').first();
        if (data.Errors && data.Errors.length > 0) {
            errorList.show();

            let errors = [];
            for (let i = 0; i < data.Errors.length; i++) {
                errors += '<li>' + data.Errors[i] + '</li>';
            }
            errorList.children('ul').html(errors);
        }
        else {
            errorList.hide();
        }

        if (data.Message) {
            window.ELApp.showNotification(data.Message);
            if (data.Message.IsSuccess) {
                var modal = form.parents('.modal').first();
                if(modal.length > 0) {
                    modal.on('hidden.bs.modal', function() {
                        form.trigger('sci-ajax-success', [form, data]);
                    });

                    modal.modal('hide');
                }
                else {
                    form.trigger('sci-ajax-success', [form, data]);
                }
            }
        }
    };
};

export class SCIModal {
    constructor(el) {
        this.el = el;
    };

    showPlaceholder(url, onAfterRender) {
        let el = this.el;
        el.modal('show');

        if (el.attr('data-loaded') !== 'true') {
            el.attr('data-loaded', 'true');

            $.ajax({
                url: url,
                type: 'GET',
                success: function (html) {
                    el.find('.modal-body').remove();
                    el.find('.modal-content').append(html);

                    if(onAfterRender) {
                        onAfterRender();
                    }

                    $.validator.unobtrusive.parse(el);
                    if ($.fn.datepicker) {
                        el.find('.sci-date-picker').datepicker({ format: 'dd.mm.yyyy' });
                    }
                }
            })
        }
    };

    resetToPlaceholder() {
        this.el.attr('data-loaded', 'false');
        this.el.find('.modal-body').html('<p class="text-center"><i class="fa fa-2x fa-refresh fa-spin"></i></p>');
        this.el.find('.modal-footer').remove();
    };
};


$(function() {
    $('.sci-ajax-grid').each(function () {
        grids.push(new SCIAjaxGrid($(this)));
    });

    $.ajaxSetup({
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status === 403) {
                window.location = '/auth/login';
            }
        }
    });

    $.extend($.validator.methods, {
        date: function (value, element) {
            return true;
        },
        range: function (value, element, param) {
            if (element.type !== 'number' && useCommaForDecimals) {
                value = value.replace(',', '.');
            }

            return this.optional(element) || (value >= param[0] && value <= param[1]);
        },
        number: function (value, element) {
            if (element.type !== 'number') {
                if (useCommaForDecimals) {
                    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
                }

                return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(value);
            }

            return this.optional(element) || /^-?(?:\d+)$/.test(value);
        }
    });

    $('[data-sci-ajax-url]').each(function () {
        var $this = $(this);
        if ($this.attr('data-initialized') === 'true') {
            return;
        }

        $this.attr('data-initialized', 'true');
        var url = $this.attr('data-sci-ajax-url');

        $.ajax({
            url: url,
            type: 'GET',
            success: function (html) {
                $this.html(html);
            }
        });
    });

    if ($.fn.datepicker) {
        $('.sci-date-picker').datepicker({ format: 'dd.mm.yyyy' });
    }

    $.connection.notificationHub.client.updateLeaveRequests = () => {
        window.ELApp.loadTotalLeaveRequests();
    };

    $.connection.hub.start();

    window.ELApp.loadTotalLeaveRequests();
});