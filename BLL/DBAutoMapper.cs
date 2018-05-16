using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DAL;
using DAL.Entities;

namespace BLL
{
    public static class DBAutoMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DB_Lot, Lot>()
                    .ForMember("Category", x => x.MapFrom(c => c.Category.Name))
                    .ForMember("Owner", x => x.MapFrom(c => c.Owner.UserId));
                cfg.CreateMap<DB_User, User>();
                cfg.CreateMap<DB_Category, Category>();
                cfg.CreateMap<DB_Subcategory, Subcategory>().ForMember("Category", x => x.MapFrom(c => c.Category.Name));
                cfg.CreateMap<Lot, DB_Lot>()
                    .ForMember("Category", x => x.Ignore())
                    .ForMember("Owner", x => x.Ignore());
            });
        }
    }
}
