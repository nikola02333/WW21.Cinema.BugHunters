using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Common
{
    public class GenericResult<T> where T : class
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }

        public List<T> DataList { get; set; }
    }
}
