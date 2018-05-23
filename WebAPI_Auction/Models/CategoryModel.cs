﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CategoryModel
    {
        public CategoryModel() { }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Subcategory> Subcategories { get; set; }
        public List<Lot> Lots { get; set; }
    }
}
