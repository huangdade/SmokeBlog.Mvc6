using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models
{
    public class OperationResult
    {
        public bool Success { get; protected set; }

        public string ErrorMessage { get; protected set; }

        protected OperationResult()
        {

        }

        public static OperationResult SuccessResult()
        {
            return new OperationResult
            {
                Success = true
            };
        }

        public static OperationResult ErrorResult(string errorMessage)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }

            return new OperationResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; protected set; }

        protected OperationResult()
            : base()
        {

        }

        public static OperationResult<T> SuccessResult(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return new OperationResult<T>
            {
                Data = data,
                Success = true
            };
        }

        public static new OperationResult<T> ErrorResult(string errorMessage)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }

            return new OperationResult<T>
            {
                ErrorMessage = errorMessage
            };
        }
    }

    public class PagedOperationResult<T> : OperationResult
    {
        public List<T> Data { get; protected set; }

        public int? Total { get; protected set; }

        public static PagedOperationResult<T> SuccessResult(List<T> data, int total)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return new PagedOperationResult<T>
            {
                Data = data,
                Success = true,
                Total = total
            };
        }

        public static new PagedOperationResult<T> ErrorResult(string errorMessage)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }

            return new PagedOperationResult<T>
            {
                ErrorMessage = errorMessage
            };
        }
    }
}
