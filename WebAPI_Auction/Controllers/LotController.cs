using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using Ninject;
using NinjectConfiguration;

namespace OnlineAuction.Controllers
{
    public class LotController : ApiController
    {
        IKernel ninjectKernel;
        ILot_Operations LOperations;
        public LotController()
        {
            ninjectKernel = new StandardKernel(new NinjectConfig());
            LOperations = ninjectKernel.Get<ILot_Operations>();
        }

        [HttpGet]
        [Route("api/lot/GetLotsBySearch")]
        public IEnumerable<Lot> GetLotsBySearch(string category, string subcategory, string keyword)
        {
            if (category == null) category = "";
            if (subcategory == null) subcategory = "";
            if (keyword == null) keyword = "";
            if (!string.IsNullOrWhiteSpace(category) || !string.IsNullOrWhiteSpace(subcategory) || !string.IsNullOrWhiteSpace(keyword))
                return LOperations.GetBySearch(category, subcategory, keyword);
            return LOperations.GetСonfirmedLots();
        }

        [HttpGet]
        [Route("api/lot/GetUnconfirmedLots")]
        public IEnumerable<Lot> GetUnconfirmedLots()
        {
            return LOperations.GetUnconfirmedLots();
        }
        [HttpGet]
        [Route("api/lot/GetConfirmedLots")]
        public IEnumerable<Lot> GetConfirmedLots()
        {
            return LOperations.GetСonfirmedLots();
        }
        [HttpGet]
        [Route("api/lot/GetEndedLots")]
        public IEnumerable<Lot> GetEndedLots()
        {
            return LOperations.GetEndedLots();
        }

        [HttpPost]
        [Route("api/lot/newLot")]
        public IHttpActionResult PostLot(Lot lot)
        {
            if (string.IsNullOrWhiteSpace(lot.Name) || string.IsNullOrWhiteSpace(lot.Specification)
                || string.IsNullOrWhiteSpace(lot.Category) || lot.Bet == 0 || lot.Duration == 0)
            {
                return BadRequest("Please, correct your inputs");
            }
            else
            {
                LOperations.SaveLot(lot);
                return Ok();
            }
        }

        [HttpPut]
        [Route("api/lot/confirm/{LotId}")]
        public void Confirm(int LotId)
        {
            LOperations.Confirm(LotId);
        }

        [HttpPut]
        [Route("api/lot/change/{Name}/{Specification}/{lotId}")]
        public IHttpActionResult Change(string Name, string Specification, int lotId)
        {
            bool result = LOperations.Change(Name, Specification, lotId);
            if (!result)
                return BadRequest("Please, input correct information");
            else
                return Ok();
        }

        [HttpPut]
        [Route("api/lot/changeBet/{bet}/{winnerId}/{LotId}")]
        public IHttpActionResult ChangeBet(int bet, int winnerId, int LotId)
        {
            /*int _bet;
            bool isInt = Int32.TryParse("ляляля", out _bet);*/
            bool result = LOperations.ChangeBet(bet, winnerId, LotId);
            if (!result)
                return BadRequest("Please, input correct bet");
            else
                return Ok();
        }

        [HttpDelete]
        [Route("api/lot/detete/{id}")]
        public void Delete(int id)
        {
            LOperations.deleteLot(id);
        }
    }
}
