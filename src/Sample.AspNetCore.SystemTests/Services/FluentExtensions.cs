﻿using System;

using Atata;

namespace Sample.AspNetCore.SystemTests.Services
{
    public static class FluentExtensions
    {
        public static TOwner RepeatFor<TOwner>(this TOwner page, Action<TOwner, string> action, string[] values)
            where TOwner : PageObject<TOwner>
        {
            foreach (var value in values)
                action.Invoke(page, value);

            return page;
        }

        public static TOwner WaitUntilExists<TOwner>(this Control<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            if(component.Exists(new SearchOptions { Timeout = new TimeSpan(0, 0, 5), IsSafely = true })) 
            {

            }

            return component.Owner;
        }


        public static TOwner StorePrice<TOwner>(this UIComponent<TOwner> component, out int value)
            where TOwner : PageObject<TOwner>
        {
            value = int.Parse(component.Content.Value, System.Globalization.NumberStyles.AllowDecimalPoint) * 100;
            return component.Owner;
        }


        public static TOwner StoreValue<TOwner>(this UIComponent<TOwner> component, out string value)
            where TOwner : PageObject<TOwner>
        {
            value = component.Content.Value;
            return component.Owner;
        }
    }
}