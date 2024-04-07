using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitRequestBuilder
    {
        private DateTime _endDate;
        private int _limit;

        public SetPartnerPromoCodeLimitRequestBuilder() { }
        

        public SetPartnerPromoCodeLimitRequestBuilder WithEndDate(DateTime dateTime)
        {
            _endDate = dateTime;
            return this;
        }

        public SetPartnerPromoCodeLimitRequestBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public SetPartnerPromoCodeLimitRequest Build()
            => new()
            {
                EndDate = _endDate,
                Limit = _limit
            };
    }
}
