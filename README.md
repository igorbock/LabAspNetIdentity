## JWT Seguro

https://blog.lsantos.dev/jwt-seguro/

## Aula de autenticação com provedores externos:

https://www.macoratti.net/20/07/aspnc_agoogle1.htm

## Servidor ASP.NET

https://www.monsterasp.net/

# Autenticação com Provedores Externos

Para autenticação com provedores externos como Google, Facebook e Twitter, o fluxo é um pouco diferente da autenticação tradicional com usuário e senha. Em vez de coletar as credenciais diretamente, o usuário será redirecionado para o provedor externo para autenticação, e o provedor retornará um token de acesso que você pode usar para autenticar o usuário em sua API.

Aqui está um exemplo de como você pode configurar e chamar a autenticação para obter o token:

## 1. Configuração do Provedor de Autenticação

Você já configurou os provedores de autenticação no `Startup.cs` ou `Program.cs`. Agora, você precisa configurar os endpoints para iniciar o processo de autenticação.

## 2. Endpoint de Login

Crie um endpoint que redireciona o usuário para o provedor de autenticação. Por exemplo:

```csharp
[HttpGet("Login/{provider}")]
public IActionResult Login(string provider)
{
    var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
    var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
    return Challenge(properties, provider);
}
```
## 3. Callback de Autenticação

Após o usuário autenticar com o provedor externo, ele será redirecionado de volta para o seu aplicativo. Crie um endpoint para lidar com o callback:

```csharp
[HttpGet("ExternalLoginCallback")]
public async Task<IActionResult> ExternalLoginCallback()
{
    var authenticateResult = await HttpContext.AuthenticateAsync("External");

    if (!authenticateResult.Succeeded)
        return BadRequest(); // Erro na autenticação

    var claims = authenticateResult.Principal.Claims;
    // Aqui você pode criar um token JWT ou outro mecanismo de autenticação
    var token = GenerateJwtToken(claims);

    return Ok(new { Token = token });
}

private string GenerateJwtToken(IEnumerable<Claim> claims)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUA_CHAVE_SECRETA"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "SeuIssuer",
        audience: "SeuAudience",
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```
## 4. Configuração do JWT

Certifique-se de que o middleware JWT esteja configurado no pipeline de requisições:

```csharp
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "SeuIssuer",
        ValidAudience = "SeuAudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUA_CHAVE_SECRETA"))
    };
});
```
## 5. Proteção de Rotas
Use o atributo [Authorize] para proteger rotas que requerem autenticação:

```csharp
[Authorize]
[HttpGet("Protected")]
public IActionResult Protected()
{
    return Ok("Esta é uma rota protegida.");
}
```
Com essa configuração, quando um usuário acessar o endpoint de login, ele será redirecionado para o provedor externo para autenticação. Após a autenticação bem-sucedida, o provedor redirecionará o usuário de volta para o seu aplicativo, onde você pode gerar um token JWT e retorná-lo ao cliente. Esse token pode então ser usado para autenticar requisições subsequentes à sua API.
