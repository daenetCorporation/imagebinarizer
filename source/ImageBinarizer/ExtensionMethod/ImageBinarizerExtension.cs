﻿using Daenet.Binarizer;
using Daenet.Binarizer.Entities;
using LearningFoundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningFoundation.Binarizer
{
    /// <summary>
    /// Extention Method class as per Learning Api Architecture
    /// </summary>
    public static class ImageBinarizerExtension
    {       
        /// <summary>
        /// Creating Object of Image Binarization in this method and adding it to Api
        /// </summary>
        /// <param name="api">This is a Api used to add module to Learning Api.It is used as a reference of Learning Api</param>
        /// <param name="configuration"></param>
        /// <returns>It return Api of Learning Api</returns>
        public static LearningApi UseImageBinarizer(this LearningApi api, BinarizerParams configuration)
        {
            ImageBinarizer module = new ImageBinarizer(configuration);
            api.AddModule(module, $"ImageBinarizer-{Guid.NewGuid()}");
            return api;
        }
    }
}
