using System;
using System.Collections.Generic;

namespace Product.Api.DomainCore.Services
{
    public class ProductValidator
    {
        public const string CODE_IS_REQUIRED = "Field Code is required.";
        public const string CODE_MUST_BE_UNIQUE = "Code should be unique.";
        public const string NAME_IS_REQUIRED = "Field Name is required.";
        public const string PRICE_SHOULD_BE_IN_RANGE = "Price should be in range 0 - 999.";
        public const string FILE_CONTENT_IVALID = "File content is not valid base64string.";

        private const int MIN_PRICE_VALUE = 0;
        private const int MAX_PRICE_VALUE = 999;

        public static void ValidateProductName(List<Fault> faults, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                faults.Add(new Fault() {Reason = nameof(name), Message = NAME_IS_REQUIRED});
            }
        }

        public static void ValidateFile(List<Fault> faults, string fileContent)
        {
            if (string.IsNullOrEmpty(fileContent)) return;

            try
            {
                Convert.FromBase64String(fileContent);
            }
            catch (Exception)
            {
                faults.Add(new Fault() { Reason = nameof(fileContent), Message = FILE_CONTENT_IVALID });
            }
        }

        public static void ValidateProductPrice(List<Fault> faults, decimal price)
        {
            bool isPriceValid = price >= MIN_PRICE_VALUE && price <= MAX_PRICE_VALUE;

            if (!isPriceValid)
            {
                faults.Add(new Fault() {Reason = nameof(price), Message = PRICE_SHOULD_BE_IN_RANGE});
            }
        }
    }
}
