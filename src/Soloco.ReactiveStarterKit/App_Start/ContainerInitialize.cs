﻿using System;
using System.Reflection;
using Soloco.ReactiveStarterKit.Common.Infrastructure.DryIoc;
using Soloco.ReactiveStarterKit.Controllers;

namespace Soloco.ReactiveStarterKit
{
    internal static class ContainerInitialize
    {
        public static IContainer RegisterApiControllers(this IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            container.RegisterServicesInNamespace(typeof(AccountController));

            return container;
        }
    }
}