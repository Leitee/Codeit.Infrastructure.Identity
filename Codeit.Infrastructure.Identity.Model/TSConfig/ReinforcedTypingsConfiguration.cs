/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.Model.TSConfig
{
    using Codeit.Infrastructure.Identity.Model.Dtos;
    using Reinforced.Typings.Ast.TypeNames;
    using Reinforced.Typings.Fluent;

    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Global(config =>
            {
                config.CamelCaseForProperties(true);
                config.UseModules(true);
                config.AutoOptionalProperties(true);
                config.ExportPureTypings(true);
            });

            builder.Substitute(typeof(System.Guid), new RtSimpleTypeName("string"));

            builder.ExportAsInterface<SettingsDTO>()
                .AutoI(false)
                .WithPublicProperties()
                .OverrideName("Settings");

            builder.ExportAsInterface<UserDTO>()
                .AutoI(false)
                .WithPublicProperties()
                .OverrideName("User");            

            builder.ExportAsInterface<RoleDTO>()
                .AutoI(false)
                .WithPublicProperties()
                .OverrideName("Role");

            builder.ExportAsInterface<SignUpDTO>()
                .AutoI(false)
                .WithPublicProperties()
                .OverrideName("SingUp");

            builder.ExportAsInterface<LoginDTO>()
                .AutoI(false)
                .WithPublicProperties()
                .OverrideName("LogIn");

            builder.ExportAsInterface<TokenDTO>()
                .AutoI(false)
                .WithPublicProperties()
                .OverrideName("Token");

            builder.ExportAsInterface<RefreshTokenDTO>()
                .AutoI(false)
                .WithPublicProperties()
                .OverrideName("RefreshToken");
        }
    }
}