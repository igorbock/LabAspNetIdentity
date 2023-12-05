global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;

global using ProviderJWT.Context;
global using ProviderJWT.Extensions;
global using ProviderJWT.Models;
global using ProviderJWT.Requirements;
global using ProviderJWT.Interfaces;
global using ProviderJWT.Helpers;
global using ProviderJWT.Services;

global using System.ComponentModel.DataAnnotations;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.IdentityModel.Tokens.Jwt;
global using System.Text.RegularExpressions;
