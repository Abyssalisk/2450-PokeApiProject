using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    public static partial class Grand
    {
        #region Typedefs
        /// <summary>
        /// Unfortunately, a lot of the stuff that this does has to be done on the heap because 
        /// if it is done on the stack, any json object of significant size will cause a stack 
        /// overflow.
        /// </summary>
        private class HuffmanCoder
        {
            private DualNode root;
            private List<DualNode> dangling = new List<DualNode>();
            //private List<DualNode> processed = new List<DualNode>();
            private class DualNode : ISerializable //FIX THIS.
            {
                internal DualNode Parent { get; set; } = null;
                internal uint numerical = 0;
                internal char? character = null;
                internal bool IsLeaf
                {
                    get => A == null && b == null;
                }
                private DualNode a;
                internal DualNode A
                {
                    get => a;
                    set
                    {
                        if (value != null)
                        {
                            value.Parent = this;
                        }
                        a = value;
                    }
                }
                private DualNode b;
                internal DualNode B
                {
                    get => b;
                    set
                    {
                        if (value != null)
                        {
                            value.Parent = this;
                        }
                        b = value;
                    }
                }
                internal DualNode()
                {

                }
                //internal DualNode(DualNode parent)
                //{
                //    Parent = parent;
                //}
            }
            internal string Encode(string text)
            {
                {
                    IOrderedEnumerable<KeyValuePair<char, uint>> orderedCommonality;
                    {
                        Dictionary<char, uint> commonalityDict = new Dictionary<char, uint>();
                        foreach (char c in text)
                        {
                            if (commonalityDict.ContainsKey(c))
                            {
                                commonalityDict[c]++;
                            }
                            else
                            {
                                commonalityDict.Add(c, 1);
                            }
                        }
                        orderedCommonality = commonalityDict.OrderByDescending((x) => x.Value);
                    }
                    foreach (var v in orderedCommonality)
                    {
                        dangling.Add(new DualNode()
                        {
                            A = null,
                            B = null,
                            character = v.Key,
                            numerical = v.Value
                        });
                    }
                }
                do
                {
                    if (dangling.Count == 1)
                    {
                        root = dangling[0];
                        dangling.Clear();
                        break;
                    }
                    dangling.Add(new DualNode()
                    {
                        A = dangling[dangling.Count - 1],
                        B = dangling[dangling.Count - 2],
                        numerical = dangling[dangling.Count - 1].numerical + dangling[dangling.Count - 2].numerical,
                        character = null
                    });
                    dangling.RemoveAt(dangling.Count - 2);
                    dangling.RemoveAt(dangling.Count - 2);
                    dangling = dangling.OrderByDescending(x => x.numerical).ToList();
                } while (true);
                throw new NotImplementedException();
                //while (dangling.Count > 0)
                //{
                //    if (dangling.Count == 1)
                //    {
                //        processed.Add(dangling.Dequeue());
                //        break;
                //    }
                //    processed.Add(new DualNode()
                //    {
                //        A = dangling.Peek(),
                //        numerical = dangling.Dequeue().numerical + dangling.Peek().numerical,
                //        B = dangling.Dequeue(),
                //        character = null
                //    });
                //}
                //if (processed.Count == 1)
                //{
                //    root = processed[0];
                //    processed.RemoveAt(0);
                //    break;
                //}
                //if (dangling.Count == 0)
                //{
                //    for (int i = processed.Count - 1; i >= 0; i--)
                //    {
                //        dangling.Enqueue(processed[i]);
                //        processed.RemoveAt(i);
                //    }
                //}
                //processed = processed.OrderBy(x => x.numerical).ToList();
            }
            private void AddBranch(DualNode node)
            {
                //dangling.Enqueue(node);
                //while (dangling.Count > 0)
                //{
                //    if (root == null)
                //    {
                //        root = dangling.Dequeue();
                //    }
                //    else
                //    {
                //        root = new DualNode()
                //        {
                //            A = root,
                //            B = dangling.Peek(),
                //            numerical = root.numerical + dangling.Dequeue().numerical
                //        };
                //    }
                //}
                //dangling.Enqueue(new DualNode()
                //{
                //    A = new DualNode()
                //    {
                //        character = a.Key,
                //        numerical = a.Value,
                //        A = null,
                //        B = null
                //    },
                //    B = new DualNode()
                //    {
                //        character = b.Key,
                //        numerical = b.Value,
                //        A = null,
                //        B = null
                //    },
                //    character = null,
                //    numerical = a.Value + b.Value
                //});
            }
        }
        #endregion
        public readonly static HashAlgorithm sha;
        static Grand()
        {
            sha = new SHA1CryptoServiceProvider();
            AppDomain.CurrentDomain.ProcessExit += GrandDestructor;
        }
        static void GrandDestructor(object sender, EventArgs e)
        {
            sha.Dispose();
        }
        public static bool VerifyPokemonLegitimacy(APIPokemonBlueprint mine, APIPokemonBlueprint theirs)
        {
            byte[] mh = sha.ComputeHash(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(mine, typeof(APIPokemonBlueprint), Formatting.None, null)));
            byte[] th = sha.ComputeHash(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(theirs, typeof(APIPokemonBlueprint), Formatting.None, null)));
            for (int i = 0; i < mh.Length; i++)
            {
                if (mh[i] != th[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static string HuffmanSerialize(string json)
        {
            return new HuffmanCoder().Encode(json);
            //oh schidt, this is gonna be super slow in C#, maybe marshall c++?
            //unsafe
            //{
            //    fixed string* s = &json;
            //}

            //Sorts how common characters are by most common at [0] and least common at [length - 1].
            throw new NotImplementedException();
        }
        public static string HuffmanSerialize<T>(T toSerialize) where T : ISerializable
        {
            throw new NotImplementedException();
        }
        public static string HuffmanDeserialize(string huff)
        {
            throw new NotImplementedException();
        }
        public static T HuffmanDeserialize<T>(string huff) where T : ISerializable
        {
            throw new NotImplementedException();
        }
    }
}