using Sabio.Data.Providers;
using Sabio.Models.Requests.Licenses;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Requests;
using Sabio.Models.Domain;
using Sabio.Data;
using Sabio.Models;

namespace Sabio.Services
{
    public class PaymentAccountService : IPaymentAccountService
    {
        IDataProvider _data = null;
        ILookUpService _lookup = null;

        public PaymentAccountService(IDataProvider data, ILookUpService lookUp)
        {
            _data = data;
            _lookup = lookUp;
        }

        #region ADD
        public int Add(PaymentAccountAddRequest model, int createdBy)
        {
            int id = 0;

            string procName = "[dbo].[PaymentAccounts_Insert]";

            _data.ExecuteNonQuery(procName,
                (SqlParameterCollection col) =>
                {
                    AddCommonParams(model, col);

                    col.AddWithValue("@CreatedBy", createdBy);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);

                }, (SqlParameterCollection col) =>
                {
                    object idObj = col["@Id"].Value;
                    Int32.TryParse(idObj.ToString(), out id);
                }
                );

            return id;
        }
        #endregion

        #region GET BY ID   
        public PaymentAccount GetById(int id)
        {
            PaymentAccount paymentAccount = null;

            string procName = "[dbo].[PaymentAccounts_Select_ById]";

            _data.ExecuteCmd(procName,
                (SqlParameterCollection col) =>
                {
                    col.AddWithValue("@Id", id);
                }, (IDataReader reader, short set) =>
                {
                    int index = 0;
                    paymentAccount = SinglePaymentAccountMapper(reader, ref index);
                }
                );

            return paymentAccount;
        }
        #endregion

        #region GET ALL (PAGINATED)   
        public Paged<PaymentAccount> Get(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[PaymentAccounts_SelectAll]";
            List<PaymentAccount> paymentAccounts = null;
            Paged<PaymentAccount> paged = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                (SqlParameterCollection col) =>
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                }, (IDataReader reader, short set) =>
                {
                    int index = 0;
                    PaymentAccount paymentAccount = SinglePaymentAccountMapper(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (paymentAccounts == null)
                    {
                        paymentAccounts = new List<PaymentAccount>();
                    }

                    paymentAccounts.Add(paymentAccount);
                }
                );

            if (paymentAccounts != null)
            {
                paged = new Paged<PaymentAccount>(paymentAccounts, pageIndex, pageSize, totalCount);
            }

            return paged;
        }
        #endregion

        #region GET BY CREATED BY 
        public List<PaymentAccount> GetByCreatedBy(int createdBy)
        {
            List<PaymentAccount> paymentAccounts = null;

            string procName = "[dbo].[PaymentAccounts_Select_ByCreatedBy]";

            _data.ExecuteCmd(procName,
                (SqlParameterCollection col) =>
                {
                    col.AddWithValue("@UserId", createdBy);
                }, (IDataReader reader, short set) =>
                {
                    int index = 0;
                    PaymentAccount paymentAccount = SinglePaymentAccountMapper(reader, ref index);

                    if (paymentAccounts == null) paymentAccounts = new List<PaymentAccount>();

                    paymentAccounts.Add(paymentAccount);
                }
                );

            return paymentAccounts;
        }
        #endregion

        #region UPDATE
        public void Update(PaymentAccountUpdateRequest request, int modifiedBy)
        {
            string procName = "[dbo].[PaymentAccounts_Update]";

            _data.ExecuteNonQuery(procName,
                (SqlParameterCollection col) =>
                {
                    AddCommonParams(request, col);

                    col.AddWithValue("@UserId", modifiedBy);
                    col.AddWithValue("@Id", request.Id);
                }
                );
        }
        #endregion

        #region DELETE
        public void DeleteById(int id)
        {
            string procName = "[dbo].[PaymentAccounts_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                (SqlParameterCollection col) =>
                {
                    col.AddWithValue("@Id", id);
                });
        } 
        #endregion

        #region ADD COMMON PARAMS   
        private static void AddCommonParams(PaymentAccountAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@VenueId", model.VenueId);
            col.AddWithValue("@AccountId", model.AccountId);
            col.AddWithValue("@PaymentTypeId", model.PaymentTypeId);
        }
        #endregion

        #region SINGLE PAYMENT ACCOUNT MAPPER
        private PaymentAccount SinglePaymentAccountMapper(IDataReader reader, ref int index)
        {
            PaymentAccount account = new PaymentAccount();

            
            account.PaymentType = new LookUp();
            account.Venue = new LookUp();

            account.Id = reader.GetSafeInt32(index++);
            account.Venue = _lookup.MapSingleLookUp(reader, ref index);
            account.AccountId = reader.GetSafeString(index++);
            account.PaymentType = _lookup.MapSingleLookUp(reader, ref index);
            account.DateCreated = reader.GetSafeDateTime(index++);
            account.DateModified = reader.GetSafeDateTime(index++);
            account.CreatedBy = reader.DeserializeObject<BaseUser>(index++);
            account.ModifiedBy = reader.DeserializeObject<BaseUser>(index++);

            return account;
        } 
        #endregion
    }
}
