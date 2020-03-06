﻿using DevExpress.DashboardCommon;
using System;

namespace WindowsFormsApp3 {
    public static class CustomPropertyExtensions {
        public static T GetValue<T>(this CustomProperties property, string name) where T : struct {
            var value = property.GetValue(name);
            if(value == null) return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}

