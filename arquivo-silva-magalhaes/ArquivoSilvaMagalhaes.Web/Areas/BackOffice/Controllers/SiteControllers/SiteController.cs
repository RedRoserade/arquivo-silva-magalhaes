﻿using ArquivoSilvaMagalhaes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice.Controllers.SiteControllers
{
    [Authorize(Roles = MembershipUtils.PortalRoleName + "," + MembershipUtils.AdminRoleName)]
    public class SiteController : BackOfficeController
    {
    }
}