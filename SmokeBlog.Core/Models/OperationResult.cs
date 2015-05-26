using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models
{
    public sealed class OperationResult
    {
        public bool Success { get; private set; }

        public string ErrorMessage { get; private set; }

        private OperationResult()
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

    public sealed class OperationResult<T>
    {
        public bool Success { get; private set; }

        public string ErrorMessage { get; private set; }

        public T Data { get; private set; }

        private OperationResult()
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

        public static OperationResult<T> ErrorResult(string errorMessage)
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
}
