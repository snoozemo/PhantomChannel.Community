﻿using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace PhantomChannel.Community.Blazor.Client;

public class CommunityStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
