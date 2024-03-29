﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;

namespace Benday.Presidents.UnitTests.Security;

    public class AuthorizationHandlerTester<TRequirement, THandler>
        where
            TRequirement : IAuthorizationRequirement
        where
            THandler : AuthorizationHandler<TRequirement>
    {
        public AuthorizationHandlerTester(
            TRequirement requirement,
            THandler handler)
        {
            _Requirement = requirement;
            _Handler = handler;
        }

        public async Task AssertSuccess()
        {
            var context = await RunHandler();

            Assert.IsTrue(context.HasSucceeded,
                "Handler should have returned success.");
        }

        public async Task AssertFailure()
        {
            var context = await RunHandler();

            Assert.IsTrue(context.HasFailed,
                "Handler should have failed.");
        }

        private TRequirement _Requirement;
        public TRequirement Requirement
        {
            get { return _Requirement; }
        }

        private THandler _Handler;
        public THandler Handler
        {
            get { return _Handler; }
        }

        private List<Claim> _Claims;
        private List<Claim> Claims
        {
            get
            {
                if (_Claims == null)
                {
                    _Claims = new List<Claim>();
                }

                return _Claims;
            }
        }

        public void AddClaim(string claimType, string claimValue)
        {
            var temp = new Claim(claimType, claimValue);

            Claims.Add(temp);
        }

        private RouteData _RouteDataInfo;
        private RouteData RouteDataInfo
        {
            get
            {
                if (_RouteDataInfo == null)
                {
                    _RouteDataInfo = new RouteData();
                }

                return _RouteDataInfo;
            }
        }

        public void AddRouteDataValue(string key, object value)
        {
            RouteDataInfo.Values.Add(key, value);
        }

        private async Task<AuthorizationHandlerContext> RunHandler()
        {
            var actionContext = new ActionContext();

            actionContext.RouteData = RouteDataInfo;
            actionContext.HttpContext = new DefaultHttpContext();
            actionContext.ActionDescriptor = new ActionDescriptor();

            var filterContext = new AuthorizationFilterContext(
                actionContext, new List<IFilterMetadata>());

            var identity = new ClaimsIdentity(Claims);

            var principal = new ClaimsPrincipal(identity);

            var context = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>()
                {
                        Requirement
                },
                principal, filterContext);


            await Handler.HandleAsync(context);

            return context;
        }        
    }
