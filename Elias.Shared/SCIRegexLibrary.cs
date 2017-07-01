using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Shared
{
    /// <summary>
    /// Library class with common regular expressions
    /// </summary>
    public static class SCIRegexLibrary
    {
        private const string LETTERS = @"[A-Za-zªµºÀ-ÖØ-öø-ƺƼ-ƿǄǆ-Ǉǉ-Ǌǌ-Ǳǳ-ʓʕ-ʯͰ-ͳͶ-ͷͻ-ͽΆΈ-ΊΌΎ-ΡΣ-ϵϷ-ҁҊ-ԣԱ-Ֆա-ևႠ-Ⴥᴀ-ᴫᵢ-ᵷᵹ-ᶚḀ-ἕἘ-Ἕἠ-ὅὈ-Ὅὐ-ὗὙὛὝὟ-ώᾀ-ᾇᾐ-ᾗᾠ-ᾧᾰ-ᾴᾶ-Άιῂ-ῄῆ-Ήῐ-ΐῖ-Ίῠ-Ῥῲ-ῴῶ-Ώⁱⁿℂℇℊ-ℓℕℙ-ℝℤΩℨK-ℭℯ-ℴℹℼ-ℿⅅ-ⅉⅎↃ-ↄⰀ-Ⱞⰰ-ⱞⱠ-Ɐⱱ-ⱼⲀ-ⳤⴀ-ⴥꙀ-ꙟꙢ-ꙭꚀ-ꚗꜢ-ꝯꝱ-ꞇꞋ-ꞌﬀ-ﬆﬓ-ﬗＡ-Ｚａ-ｚ]|\ud801[\udc00-\udc4f]|\ud835[\udc00-\udc54\udc56-\udc9c\udc9e-\udc9f\udca2\udca5-\udca6\udca9-\udcac\udcae-\udcb9\udcbb\udcbd-\udcc3\udcc5-\udd05\udd07-\udd0a\udd0d-\udd14\udd16-\udd1c\udd1e-\udd39\udd3b-\udd3e\udd40-\udd44\udd46\udd4a-\udd50\udd52-\udea5\udea8-\udec0\udec2-\udeda\udedc-\udefa\udefc-\udf14\udf16-\udf34\udf36-\udf4e\udf50-\udf6e\udf70-\udf88\udf8a-\udfa8\udfaa-\udfc2\udfc4-\udfcb]";

        public const string EMAIL = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
        public const string PASSWORD = @"^(?=.*[A-Z])(?=.*[^a-zA-Z\d])(?=.*[0-9])(?=.*[a-z]).{8,}$";
        public const string PHONE = @"^(\+?)\d+$";
        public const string USERNAME = @"^[A-Za-z0-9_]+$";
        public const string NAME = @"^" + LETTERS + "+$";
        public const string NUMERIC = @"^\d+$";
    }
}
