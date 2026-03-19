using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Shared.Models
{
    /// <summary>
    /// Result Pattern implementasyonu.
    /// 
    /// Result Pattern: Exception fırlatmak yerine başarı/hata durumları
    /// explicit tiplerle ifade edilir. Bu yaklaşım:
    /// - Performansı artırır (exception stack trace maliyeti yok)
    /// - Hata yönetimini zorlar (caller Result'ı kontrol etmek zorunda)
    /// - Kod okunabilirliğini artırır
    /// 
    /// Single Responsibility: Her Result nesnesi ya başarı ya hata taşır, ikisi birden olamaz.
    /// </summary>
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("Success result cannot have an error.");

            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("Failure result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Failure result has no value.");

        public static implicit operator Result<TValue>(TValue value) => Success(value);
    }
}
