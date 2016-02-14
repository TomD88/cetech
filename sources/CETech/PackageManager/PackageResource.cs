using System.IO;
using MsgPack.Serialization;
using YamlDotNet.RepresentationModel;

namespace CETech
{
    public class PackageResource
    {
        public static readonly StringId Type = new StringId("package");

        public struct PackagePack
        {
            public StringId[] type;
            public StringId[][] names;
        }

        public static unsafe void compile(ResourceCompiler.CompilatorAPI capi)
        {

            TextReader input = new StreamReader(capi.ResourceFile);
            var yaml = new YamlStream();
            yaml.Load(input);

            var rootNode = yaml.Documents[0].RootNode as YamlMappingNode;

            var pack = new PackagePack();
            pack.type = new StringId[rootNode.Children.Count];
            pack.names = new StringId[rootNode.Children.Count][];

            int idx = 0;
            foreach (var type in rootNode.Children)
            {
                var typestr = type.Key as YamlScalarNode;
                var sequence = type.Value as YamlSequenceNode;

                var typeid = new StringId(typestr.Value);

                pack.type[idx] = typeid;
                pack.names[idx] = new StringId[sequence.Children.Count];

                int name_idx = 0;
                foreach (var name in sequence.Children)
                {
                    var nameid = new StringId(((YamlScalarNode) name).Value);

                    pack.names[idx][name_idx] = nameid;
                    ++name_idx;
                }

                ++idx;
            }

            var serializer = MessagePackSerializer.Get<PackagePack>();
            serializer.Pack(capi.BuildFile, pack);
        }

        public static object ResourceLoader(Stream input)
        {
            var serializer = MessagePackSerializer.Get<PackagePack>();
            return serializer.Unpack(input);
        }

        public static void ResourceOffline(object data)
        {
        }

        public static void ResourceOnline(object data)
        {
        }

        public static void ResourceUnloader(object data)
        {
        }
    }
}