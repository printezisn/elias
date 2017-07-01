const path = require('path');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const WebpackCleanupPlugin = require('webpack-cleanup-plugin');
const webpack = require('webpack');

module.exports = {
    watch: true,
    entry: {
        site: path.resolve(__dirname, 'Content/js/site.js')
    },
    output: {
        path: path.resolve(__dirname, 'Content/build/'),
        filename: '[name]-[hash].min.js'
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                use: {
                    loader: 'babel-loader',
                    options: { presets: 'es2015', cacheDirectory: true }
                },
                exclude: [path.resolve(__dirname, 'node_modules')]
            },
            {
                test: /\.scss$/,
                loader: ExtractTextPlugin.extract({
                    use: ['css-loader', 'sass-loader']
                }),
                exclude: [path.resolve(__dirname, 'node_modules')]
            },
            {
                test: /\.(png|jpg|jpeg|gif|svg)$/,
                use: {
                    loader: 'file-loader',
                    options: { name: 'img/[name].[ext]' }
                }
            },
            {
                test: /\.(ttf|woff|woff2|eot)$/,
                use: {
                    loader: 'file-loader',
                    options: { name: 'fonts/[name].[ext]' }
                }
            }
        ]
    },
    plugins: [
        new WebpackCleanupPlugin(),
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery'
        }),
        new ExtractTextPlugin({
            filename: '[name]-[hash].min.css',
            allChunks: true
        })
    ]
};