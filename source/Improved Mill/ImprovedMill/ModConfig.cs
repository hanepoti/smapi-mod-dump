﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedMill
{
    class ModConfig
    {
        public bool ProcessCorn { get; set; } = true ;
        public float SugarForCorn { get; set; } = 1.5f;
        public float FlourForCorn { get; set; } = 1.5f;
        public bool ProcessWheat { get; set; } = false;
        public float SugarForWheat { get; set; } = 0f;
        public float FlourForWheat { get; set; } = 1f;
        public bool ProcessAmaranth { get; set; } = true;
        public float SugarForAmaranth { get; set; } = 0f;
        public float FlourForAmaranth { get; set; } = 2f;
        public bool ProcessBeet { get; set; } = false;
        public float SugarForBeet { get; set; } = 3f;
        public float FlourForBeet { get; set; } = 0f;
        public ModConfig()
        {
        }
    }
}
