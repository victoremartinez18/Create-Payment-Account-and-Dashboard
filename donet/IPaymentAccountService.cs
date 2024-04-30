using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IPaymentAccountService
    {
        int Add(PaymentAccountAddRequest model, int createdBy);
        PaymentAccount GetById(int id);
        Paged<PaymentAccount> Get(int pageIndex, int pageSize);
        List<PaymentAccount> GetByCreatedBy(int createdBy);
        void Update(PaymentAccountUpdateRequest request, int modifiedBy);
        void DeleteById(int id);
    }
}