using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
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
        public IEnumerable<LotModel> GetLotsBySearch(string category, string subcategory, string keyword)
        {
            if (category == null) category = "";
            if (subcategory == null) subcategory = "";
            if (keyword == null) keyword = "";
            if (!string.IsNullOrWhiteSpace(category) || !string.IsNullOrWhiteSpace(subcategory) || !string.IsNullOrWhiteSpace(keyword))
            {
                IEnumerable<Lot> tempLots = LOperations.GetBySearch(category, subcategory, keyword);
                IEnumerable<LotModel> lots = Mapper.Map<IEnumerable<Lot>, IEnumerable<LotModel>>(tempLots);
                return lots;
            }
            IEnumerable<Lot> tempCLots = LOperations.GetСonfirmedLots();
            IEnumerable<LotModel> clots = Mapper.Map<IEnumerable<Lot>, IEnumerable<LotModel>>(tempCLots);
            return clots;
        }

        [HttpGet]
        [Route("api/lot/GetUnconfirmedLots")]
        public IEnumerable<LotModel> GetUnconfirmedLots()
        {
            IEnumerable<Lot> tempLots = LOperations.GetUnconfirmedLots();
            IEnumerable<LotModel> lots = Mapper.Map<IEnumerable<Lot>, IEnumerable<LotModel>>(tempLots);
            return lots;
        }
        [HttpGet]
        [Route("api/lot/GetConfirmedLots")]
        public IEnumerable<LotModel> GetConfirmedLots()
        {
            IEnumerable<Lot> tempLots = LOperations.GetСonfirmedLots();
            IEnumerable<LotModel> lots = Mapper.Map<IEnumerable<Lot>, IEnumerable<LotModel>>(tempLots);
            return lots;
        }
        [HttpGet]
        [Route("api/lot/GetEndedLots")]
        public IEnumerable<LotModel> GetEndedLots()
        {
            IEnumerable<Lot> tempLots = LOperations.GetEndedLots();
            IEnumerable<LotModel> lots = Mapper.Map<IEnumerable<Lot>, IEnumerable<LotModel>>(tempLots);
            return lots;
        }

        [HttpPost]
        [Route("api/lot/newLot")]
        public IHttpActionResult PostLot(LotModel _lot)
        {
            if (string.IsNullOrWhiteSpace(_lot.Name) || string.IsNullOrWhiteSpace(_lot.Specification)
                || string.IsNullOrWhiteSpace(_lot.Category) || _lot.Bet == 0 || _lot.Duration == 0)
            {
                return BadRequest("Please, correct your inputs");
            }
            else
            {
                Lot lot = Mapper.Map<LotModel, Lot>(_lot);
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
