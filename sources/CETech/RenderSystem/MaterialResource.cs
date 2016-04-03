using System.IO;
using CETech.Develop;
using MsgPack.Serialization;
using SharpBgfx;
using YamlDotNet.RepresentationModel;

namespace CETech
{
    /// <summary>
    ///     Package resource
    /// </summary>
    public class MaterialResource
    {
        /// <summary>
        ///     Resource type
        /// </summary>
        public static readonly long Type = StringId.FromString("material");

#if CETECH_DEVELOP

        /// <summary>
        ///     Resource compiler
        /// </summary>
        /// <param name="capi">Compiler api</param>
        public static void Compile(ResourceCompiler.CompilatorApi capi)
        {
            TextReader input = new StreamReader(capi.ResourceFile);
            var yaml = new YamlStream();
            yaml.Load(input);

            var rootNode = yaml.Documents[0].RootNode as YamlMappingNode;
            var shader_name = ((YamlScalarNode) rootNode.Children[new YamlScalarNode("shader")]).Value;

            var resource = new Resource { shader_name = StringId.FromString(shader_name) };

            if (rootNode.Children.ContainsKey(new YamlScalarNode("textures"))) {
                var textures = (YamlMappingNode)rootNode.Children[new YamlScalarNode("textures")];

                resource.unforms_name = new string[textures.Children.Count];
                resource.texture = new string[textures.Children.Count];

                var idx = 0;
                foreach (var texture in textures.Children)
                {
                    resource.unforms_name[idx] = ((YamlScalarNode) texture.Key).Value;
                    resource.texture[idx] = ((YamlScalarNode)texture.Value).Value;

                    ++idx;
                }

            }
            else
            {
                resource.unforms_name = new string[0];
                resource.texture = new string[0];
            }

            var serializer = MessagePackSerializer.Get<Resource>();
            serializer.Pack(capi.BuildFile, resource);

            capi.add_dependency(shader_name + ".shader");
        }
#endif

        /// <summary>
        ///     Resource loader
        /// </summary>
        /// <param name="input">Resource data stream</param>
        /// <returns>Resource data</returns>
        public static object ResourceLoader(Stream input)
        {
            var serializer = MessagePackSerializer.Get<Resource>();
            var resource = serializer.Unpack(input);

            var mat_inst = new MaterialInstance { resource = resource, texture_uniform = new Uniform[resource.texture.Length], texture_resource = new TextureResource.Resource[resource.texture.Length] };

            return mat_inst;
        }

        /// <summary>
        ///     Resource offline.
        /// </summary>
        /// <param name="data">Data</param>
        public static void ResourceOffline(object data)
        {
        }

        /// <summary>
        ///     Resource online
        /// </summary>
        /// <param name="data">Data</param>
        public static void ResourceOnline(object data)
        {
            var resource = (MaterialInstance) data;
            resource.instance = ResourceManager.Get<ShaderResource.ShaderInstance>(ShaderResource.Type,
                resource.resource.shader_name);

            var idx = 0;
            foreach (var uniform_name in resource.resource.texture)
            {
                resource.texture_uniform[idx] = new Uniform(uniform_name, UniformType.Int1);
                resource.texture_resource[idx] = ResourceManager.Get<TextureResource.Resource>(TextureResource.Type, StringId.FromString(uniform_name));
                ++idx;
            }
        }

        /// <summary>
        ///     Resource unloader
        /// </summary>
        /// <param name="data">data</param>
        public static void ResourceUnloader(object data)
        {
        }

        public struct Resource
        {
            public long shader_name;
            public string[] unforms_name;
            public string[] texture;
        }

        public class MaterialInstance
        {
            public ShaderResource.ShaderInstance instance;
            public Resource resource;
            public Uniform[] texture_uniform;
            public TextureResource.Resource[] texture_resource;
        }

        public static object ResourceReloader(long name, object new_data)
        {
            // TODO: !!!

            var old = ResourceManager.Get<MaterialInstance>(Type, name);
            var resource = (MaterialInstance)new_data;
            old.resource = resource.resource;
            old.instance = ResourceManager.Get<ShaderResource.ShaderInstance>(ShaderResource.Type,
                resource.resource.shader_name);

            old.texture_uniform = resource.texture_uniform;
            old.texture_resource = resource.texture_resource;

            if (resource.resource.texture.Length > 0)
            {
                var idx = 0;
                foreach (var uniform_name in resource.resource.texture)
                {

                    old.texture_uniform[idx] = new Uniform(uniform_name, UniformType.Int1);
                    old.texture_resource[idx] = ResourceManager.Get<TextureResource.Resource>(TextureResource.Type,
                        StringId.FromString(uniform_name));
                    ++idx;
                }
            }

            return old;
        }
    }
}