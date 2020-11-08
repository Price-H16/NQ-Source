﻿using System;
using Autofac;
using ChickenAPI.Plugins;
using OpenNos.Core;
using OpenNos.Core.Extensions;
using OpenNos.GameObject._Guri;

namespace Plugins.BasicImplementations.Guri
{
    public class GuriPlugin : IGamePlugin
    {
        private readonly IContainer _container;
        private readonly IGuriHandlerContainer _handlers;

        public GuriPlugin(IGuriHandlerContainer handlers, IContainer container)
        {
            _handlers = handlers;
            _container = container;
        }

        public string Name => nameof(GuriPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            foreach (var handlerType in typeof(GuriPlugin).Assembly.GetTypesImplementingInterface<IGuriHandler>())
                try
                {
                    var tmp = _container.Resolve(handlerType);
                    if (!(tmp is IGuriHandler real)) continue;

                    Logger.Log.InfoFormat($"[GURI][ADD_HANDLER] {handlerType}");
                    _handlers.Register(real).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Logger.Log.Error("[GURI][FAIL_ADD]", e);
                }
        }

        public void OnLoad()
        {
        }
    }
}