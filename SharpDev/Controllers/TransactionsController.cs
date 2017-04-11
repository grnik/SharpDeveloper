using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BusnessServices;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BusnessEntities;
using BusnessServices.Exceptions;
using Ninject;

namespace SharpDev.Controllers
{
    [AttributeRouting.RoutePrefix("v1/Transactions")]
    public class TransactionsController : BaseController
    {
        private ITransactionService transactionService;

        public TransactionsController()
            : base()
        {
            transactionService = AppKernel.Get<ITransactionService>();
        }

        [GET("GET")]
        [GET("All")]
        [System.Web.Mvc.HttpGet]
        //public IEnumerable<TransactionOutEntity> GetAll()
        public IHttpActionResult GetAll()
        {
            try
            {
                return Ok<IEnumerable<TransactionOutEntity>>(transactionService.GetByLogin(CurrentUser.Email).OrderBy(t => t.Date).ToList());
            }
            catch (MyException e)
            {
                return Results(e);
            }
            catch (System.Exception e)
            {
                return Results(e);
            }
        }

        // POST: Transactions/Create
        [POST("POST")]
        [POST("CREATE")]
        public IHttpActionResult Post([FromBody]TransactionInEntity newTransaction)
        {
            try
            {
                if (string.IsNullOrEmpty(newTransaction.ToLogin))
                {
                    throw new MyException("You must select the payment receiver");
                }
                return Ok(transactionService.Insert(newTransaction, CurrentUser));
            }
            catch (MyException e)
            {
                return Results(e);
            }
            catch (System.Exception e)
            {
                return Results(e);
            }
        }
    }
}
