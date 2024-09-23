﻿namespace Ecommerce.V1.Commons.ResponseModels
{
    public class ResponseModel<T>
    {
        public bool Status { get; set; }
        public string Error { get; set; }
        public T? Data { get; set; }
        public bool? GlobalError { get; set; }
    }
}
