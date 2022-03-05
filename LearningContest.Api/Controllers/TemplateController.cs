using CommonInfrastructure.Attribute;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LearningContest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private LearningContest.Persistence.LearningContestDbContext _context { get; set; }
        private IWebHostEnvironment _webHostEnvironment { get; set; }
        public TemplateController(Persistence.LearningContestDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _webHostEnvironment = env;
        }


        [HttpGet("CreateFiles/{entityName}")]
        public async Task<ActionResult> CreateFiles(string entityName, string entitySchema)
        {
            var assemblyToScan = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblyToScan.SelectMany(s => s.GetTypes());

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Users\10\Desktop\temp.txt"); 
            Type entity = types.Where(c => c.FullName.StartsWith("LearningContest.Domain.Entities", StringComparison.InvariantCultureIgnoreCase) &&
                                                    c.FullName.EndsWith(entityName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (entity == null)
            {
                return BadRequest();
            }

            string templateName = "BasketTag";
            string templateSchema = "Stock";

            var apiRootPath = _webHostEnvironment.ContentRootPath;
            var applicationRootPath = GetApplicationRootPath(apiRootPath);

            string featuresTemplateFolder = $@"{applicationRootPath}Features\{templateName}";
            string featuresFolder = $@"{applicationRootPath}Features\{entityName}";
            string commandsFolder = featuresFolder + @"\Commands";
            string queriesFolder = featuresFolder + @"\Queries";
            string addFolder = commandsFolder + @"\Add";
            string deleteFolder = commandsFolder + @"\Delete";
            string updateFolder = commandsFolder + @"\Update";
            string getByIdFolder = queriesFolder + @"\GetById";
            string listFolder = queriesFolder + @"\List";
            //CreateDirectory(featuresFolder);

            //Create Features Directory
            foreach (string dirPath in Directory.GetDirectories(featuresTemplateFolder, "*", SearchOption.AllDirectories))
            {
                CreateDirectory(dirPath.Replace(featuresTemplateFolder, featuresFolder));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string templateFile in Directory.GetFiles(featuresTemplateFolder, "*.*", SearchOption.AllDirectories))
            {
                var newFileAddress = templateFile.Replace(featuresTemplateFolder, featuresFolder);
                //System.IO.File.Copy(templateFiles, fileAddress, true);

                if (newFileAddress.EndsWith("AddCommand.cs"))
                {

                    var addCommandText = CreateAddCommandText(entity);
                    System.IO.File.WriteAllText(newFileAddress, addCommandText);

                }
                else if (newFileAddress.EndsWith("UpdateCommand.cs"))
                {
                    var updateCommandText = CreateUpdateCommandText(entity);
                    System.IO.File.WriteAllText(newFileAddress, updateCommandText);
                }
                else if (newFileAddress.EndsWith("GetByIdQuery.cs"))
                {
                    var getByIdText = CreateGetByIdQueryText(entity);
                    System.IO.File.WriteAllText(newFileAddress, getByIdText);
                }
                else
                {
                    string content = System.IO.File.ReadAllText(templateFile);
                    content = content.Replace(templateName, entityName);
                    content = content.Replace(templateSchema, entitySchema);

                    System.IO.File.WriteAllText(newFileAddress, content);
                }
            }
            //IRepository Interface
            var repoSchema = entitySchema.ToLower() == "general" ? "" : (entitySchema + @"\");
            string repoInterfaceTemplateFile = $@"{applicationRootPath}Contracts\Persistence\{templateSchema}\I{templateName}Repository.cs";
            string repoInterfaceFile = $@"{applicationRootPath}Contracts\Persistence\{repoSchema}I{entityName}Repository.cs";
            if (!System.IO.File.Exists(repoInterfaceFile))
            {
                string repoInterfaceContent = System.IO.File.ReadAllText(repoInterfaceTemplateFile);
                repoInterfaceContent = repoInterfaceContent.Replace(templateName, entityName);
                repoInterfaceContent = repoInterfaceContent.Replace(templateSchema, entitySchema);

                string repoInterfaceDirectory = repoInterfaceFile.Substring(0, repoInterfaceFile.LastIndexOf(@"\"));
                CreateDirectory(repoInterfaceDirectory);

                System.IO.File.WriteAllText(repoInterfaceFile, repoInterfaceContent);
            }

            //Repository 
            string persisetnceRootPath = GetPersistenceRootPath(apiRootPath);
            string repoTemplateFile = $@"{persisetnceRootPath}Repositories\{templateSchema}\{templateName}Repository.cs";
            string repoFile = $@"{persisetnceRootPath}Repositories\{repoSchema}{entityName}Repository.cs";
            if (!System.IO.File.Exists(repoFile))
            {
                string repoContent = System.IO.File.ReadAllText(repoTemplateFile);
                repoContent = repoContent.Replace(templateName, entityName);
                repoContent = repoContent.Replace(templateSchema, entitySchema);

                string repoDirectory = repoFile.Substring(0, repoFile.LastIndexOf(@"\"));
                CreateDirectory(repoDirectory);

                System.IO.File.WriteAllText(repoFile, repoContent);
            }

            return Ok();
        }

        [HttpGet("DeleteFiles/{entityFullName}")]
        public async Task<ActionResult> DeleteFiles(string entityFullName)
        {
            var entityInfo = entityFullName.Split('.');
            string entityName = entityInfo[entityInfo.Length - 1];
            var apiRootPath = _webHostEnvironment.ContentRootPath;
            var applicationRootPath = GetApplicationRootPath(apiRootPath);

            string featuresFolder = applicationRootPath + @"Features\" + entityName;
            if (System.IO.Directory.Exists(featuresFolder))
            {
                System.IO.Directory.Delete(featuresFolder, true);
            }
            return Ok();
        }

        private string GetApplicationRootPath(string apiRootPath)
        {
            return apiRootPath.Substring(0, apiRootPath.LastIndexOf(@"\")) + @"\LearningContest.Application\";
        }
        private string GetPersistenceRootPath(string apiRootPath)
        {
            return apiRootPath.Substring(0, apiRootPath.LastIndexOf(@"\")) + @"\LearningContest.Persistence\";
        }

        private void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        private string CreateAddCommandText(Type entity)
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"LearningContest.Application.Features.{entity.Name}.Commands.Add")).NormalizeWhitespace();

            // Add System using statement: (using System)
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("MediatR")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("LearningContest.Application.Responses")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.ComponentModel.DataAnnotations")));

            //  Create a class: (class Order)
            var classDeclaration = SyntaxFactory.ClassDeclaration("AddVm");
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            foreach (var prop in entity.GetProperties())
            {
                var propertyDeclaration = CreatePropertyForInsert(prop);
                if (propertyDeclaration != null)
                {
                    // Add the field, the property and method to the class.
                    classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
                }
            }

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);
            ClassDeclarationSyntax commandClassDeclaration = SyntaxFactory.ClassDeclaration("AddCommand");
            commandClassDeclaration = commandClassDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            commandClassDeclaration = commandClassDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("IRequest<BaseResponse<int>>")));

            foreach (var prop in entity.GetProperties())
            {
                var propertyDeclaration = CreatePropertyForInsert(prop);
                // Add the field, the property and method to the class.
                if (propertyDeclaration != null)
                {
                    commandClassDeclaration = commandClassDeclaration.AddMembers(propertyDeclaration);
                }

            }


            @namespace = @namespace.AddMembers(commandClassDeclaration);

            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();
            return code;

        }

        private PropertyDeclarationSyntax CreatePropertyForInsert(PropertyInfo prop)
        {
            EntityConfigAttribute configAtt = prop.GetCustomAttribute(typeof(EntityConfigAttribute)) as EntityConfigAttribute;
            //to appear on insert 
            if (configAtt != null)
            {
                if (configAtt.Insert == ConfigItemState.Exist)
                {
                    return CreateProperty(prop);
                }
            }
            return null;
        }
        private PropertyDeclarationSyntax CreatePropertyForUpdate(PropertyInfo prop)
        {
            var configAtt = prop.GetCustomAttribute(typeof(EntityConfigAttribute)) as EntityConfigAttribute;
            //to appear on insert 
            if (configAtt != null)
            {
                if (configAtt.Update == ConfigItemState.Exist)
                {
                    return CreateProperty(prop);
                }
            }
            return null;
        }
        private PropertyDeclarationSyntax CreatePropertyForDto(PropertyInfo prop)
        {
            var configAtt = prop.GetCustomAttribute(typeof(EntityConfigAttribute)) as EntityConfigAttribute;
            //to appear on insert 
            if (configAtt != null)
            {
                if (configAtt.Dto == ConfigItemState.Exist)
                {
                    var name = SyntaxFactory.ParseName("Sieve(CanFilter = true, CanSort = true)");
                    var attribute = SyntaxFactory.Attribute(name);
                    var attributeList = new SeparatedSyntaxList<AttributeSyntax>();
                    attributeList = attributeList.Add(attribute);
                    var attList = SyntaxFactory.AttributeList(attributeList);
                    var propertyDeclaration = CreateProperty(prop);

                    return propertyDeclaration.AddAttributeLists(attList);
                }
            }
            return null;
        }

        private PropertyDeclarationSyntax CreateProperty(PropertyInfo prop)
        {             
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type type = prop.PropertyType.GetGenericArguments()[0];
                return SyntaxFactory.PropertyDeclaration(SyntaxFactory.NullableType(SyntaxFactory.ParseTypeName(type.Name)), prop.Name)
                                                                          .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                                                          .AddAccessorListAccessors(
                                                 SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                                 SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
            }
            else
            {
                return SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(prop.PropertyType.Name), prop.Name)
                                                                         .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                                                         .AddAccessorListAccessors(
                                                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
            }

            
        }

        private string CreateUpdateCommandText(Type entity)
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"LearningContest.Application.Features.{entity.Name}.Commands.Update")).NormalizeWhitespace();

            // Add System using statement: (using System)
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("MediatR")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("LearningContest.Application.Responses")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.ComponentModel.DataAnnotations")));

            //  Create a class: (class Order)
            var classDeclaration = SyntaxFactory.ClassDeclaration("UpdateVm");
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            foreach (var prop in entity.GetProperties())
            {
                var propertyDeclaration = CreatePropertyForUpdate(prop);
                if (propertyDeclaration != null)
                {
                    // Add the field, the property and method to the class.
                    classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
                }
            }

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);
            ClassDeclarationSyntax commandClassDeclaration = SyntaxFactory.ClassDeclaration("UpdateCommand");
            commandClassDeclaration = commandClassDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            commandClassDeclaration = commandClassDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("IRequest<BaseResponse<bool>>")));

            foreach (var prop in entity.GetProperties())
            {
                var propertyDeclaration = CreatePropertyForUpdate(prop);
                // Add the field, the property and method to the class.
                if (propertyDeclaration != null)
                {
                    commandClassDeclaration = commandClassDeclaration.AddMembers(propertyDeclaration);
                }

            }


            @namespace = @namespace.AddMembers(commandClassDeclaration);

            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();
            return code;

        }

        private string CreateGetByIdQueryText(Type entity)
        {
            string dtoName = entity.Name + "Dto";

            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"LearningContest.Application.Features.{entity.Name}.Queries.GetById")).NormalizeWhitespace();

            // Add System using statement: (using System)
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("MediatR")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("LearningContest.Application.Responses")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.ComponentModel.DataAnnotations")));
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Sieve.Attributes")));

            //ایجاد کلاس GetByIdQuery
            ClassDeclarationSyntax getByIdClassDeclaration = SyntaxFactory.ClassDeclaration("GetByIdQuery");
            getByIdClassDeclaration = getByIdClassDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            getByIdClassDeclaration = getByIdClassDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"IRequest<BaseResponse<{dtoName}>>")));

            foreach (var prop in entity.GetProperties())
            {
                if (prop.Name.ToLower() == "id")
                {
                    var propertyDeclaration = CreateProperty(prop);
                    if (propertyDeclaration != null)
                    {
                        // Add the field, the property and method to the class.
                        getByIdClassDeclaration = getByIdClassDeclaration.AddMembers(propertyDeclaration);
                    }
                }
            }


            @namespace = @namespace.AddMembers(getByIdClassDeclaration);

            //  Create a class: (class Order)
            var dtoClassDeclaration = SyntaxFactory.ClassDeclaration(dtoName);
            dtoClassDeclaration = dtoClassDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            foreach (var prop in entity.GetProperties())
            {
                var propertyDeclaration = CreatePropertyForDto(prop);
                if (propertyDeclaration != null)
                {
                    // Add the field, the property and method to the class.
                    dtoClassDeclaration = dtoClassDeclaration.AddMembers(propertyDeclaration);
                }
            }
            @namespace = @namespace.AddMembers(dtoClassDeclaration);


            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();
            return code;

        }
    }
}
