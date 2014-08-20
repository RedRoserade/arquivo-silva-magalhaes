﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ArquivoSilvaMagalhaes.Common
{
    public class HtmlEncoder
    {
        public static string Encode(string original, params string[] forbiddenTags)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(original as string);

            var elementsToRemove = new List<HtmlNode>();

            //foreach (var tag in doc.DocumentNode.Descendants("script"))
            foreach (var tag in doc.DocumentNode.DescendantsAndSelf())
            {
                if (forbiddenTags.Contains(tag.Name))
                {
                    elementsToRemove.Add(tag);
                }
            }

            foreach (var node in elementsToRemove)
            {
                node.Remove();
            }

            var sw = new StringWriter();

            doc.Save(sw);

            sw.Flush();
            var outHtml = sw.ToString();

            sw.Close();
            return outHtml;
        }

    }
}