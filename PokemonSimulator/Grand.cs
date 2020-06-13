using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using Org.BouncyCastle.Asn1.Cms;
using Google.Protobuf;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
            #region Typedefs
            /// <summary>
            /// DualNode is used to make the decryption/encryption tree for Huffman coding.
            /// </summary>
            private class DualNode
            {
                #region Fields
                #region InstanceFields
                /// <summary>
                /// Sum of the number values in the A and B nodes, if this node is a leaf, 
                /// it is the number of instances of the character that occured in the string.
                /// </summary>
                internal uint numerical = 0;
                /// <summary>
                /// The character of this leaf, or null if this is a branch.
                /// </summary>
                internal char? character = null;
                #endregion

                #region Properties
                /// <summary>
                /// Whether or not this node is a leaf (It's a leaf if it has no child nodes, and has a character)
                /// </summary>
                internal bool IsLeaf
                {
                    get
                    {
                        if (A == null && b == null)
                        {
                            if (character != null)
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("Error: This node has no children yet is not assigned a character.");
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                /// <summary>
                /// The parent of this node
                /// </summary>
                internal DualNode Parent { get; set; } = null;
                /// <summary>
                /// Property "A" storage field
                /// </summary>
                private DualNode a;
                /// <summary>
                /// The 1st child of this node.
                /// </summary>
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
                /// <summary>
                /// Property "B" storage field
                /// </summary>
                private DualNode b;
                /// <summary>
                /// The 2nd child of this node.
                /// </summary>
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
                #endregion
                #endregion

                #region Constructors
                /// <summary>
                /// Used internally with object initializers.
                /// </summary>
                internal DualNode()
                {

                }
                /// <summary>
                /// Creates a DualNode object (including its subtree) provided in the serialized 
                /// information.
                /// </summary>
                /// <param name="serialized">The serialized data. 
                /// This is assumed to be in proper form.</param>
                internal DualNode(string serialized)
                {
                    //[N:3C:NL
                    numerical = uint.Parse(serialized.Substring(3, serialized.IndexOf("C:") - 3));
                    if (serialized.Substring(serialized.IndexOf("C:") + 2, 2) == "NL")
                    {
                        character = null;
                    }
                    else
                    {
                        character = serialized.ElementAt(serialized.IndexOf("C:") + 2);
                    }
                    string trimmed = serialized.Substring(serialized.IndexOf("A:"), serialized.Length - (serialized.IndexOf("A:") + 1));
                    if (trimmed == "A:NLB:NL")
                    {
                        A = null;
                        B = null;
                        return;
                    }
                    int indexOfB = 0;
                    int count = 0;
                    int index = 2;
                    do
                    {
                        if (trimmed[index] == '[' && trimmed[index - 2] != 'C')
                        {
                            count++;
                        }
                        else if (trimmed[index] == ']' && trimmed[index - 2] != 'C')
                        {
                            count--;
                        }
                        index++;
                    } while (count > 0 && index < trimmed.Length);
                    indexOfB = index;
                    string sa = trimmed.Substring(2, indexOfB - 2);
                    string sb = trimmed.Substring(indexOfB + 2, trimmed.Length - (indexOfB + 2));
                    A = sa == "NL" ? null : new DualNode(sa);
                    B = sb == "NL" ? null : new DualNode(sb);
                }
                #endregion

                #region Methods
                #region Member Methods
                internal string Serialize()
                {
                    return $"[N:{this.numerical}C:" +
                        $"{((this.character == null) ? "NL" : this.character.Value.ToString())}" +
                        $"A:{(this.A == null ? "NL" : A.Serialize())}" +
                        $"B:{(this.B == null ? "NL" : B.Serialize())}]";
                }
                #endregion

                #region Class Methods
                internal static DualNode Deserialize(string serialized)
                {
                    if (serialized[0] == '[' && serialized[serialized.Length - 1] == ']')
                    {
                        return new DualNode(serialized.Substring(1, serialized.Length - 2));
                    }
                    else
                    {
                        throw new ArgumentException("The provided string does not appear to be in the proper form.");
                    }
                }
                #endregion
                #endregion
            }
            #endregion

            private DualNode root;
            private Queue<DualNode> dangling = new Queue<DualNode>();
            //private List<DualNode> processed = new List<DualNode>();
            internal string Encode(string text)
            {
                StringBuilder result = new StringBuilder(string.Empty);
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
                        orderedCommonality = commonalityDict.OrderBy((x) => x.Value);
                    }
                    foreach (KeyValuePair<char, uint> v in orderedCommonality)
                    {
                        dangling.Enqueue(new DualNode()
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
                        root = dangling.Dequeue();
                        break;
                    }
                    dangling.Enqueue(new DualNode()
                    {
                        A = dangling.Peek(),
                        numerical = dangling.Dequeue().numerical + dangling.Peek().numerical,
                        B = dangling.Dequeue(),
                        character = null
                    });
                    dangling = new Queue<DualNode>(dangling.OrderBy(x => x.numerical));
                } while (true);
                if (root?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A != null)
                {
                    throw new Exception("Error: One or more characters could be encoded as a \'\\0\'.");
                }
                result.Append(root.Serialize() + "$D");
                Dictionary<char, string> encodingDictionary = new Dictionary<char, string>();
                void Tree(DualNode r, string soFar)
                {
                    if (r.IsLeaf)
                    {
                        encodingDictionary.Add(r.character.Value, soFar);//new string(soFar.ToCharArray().Reverse().ToArray())
                    }
                    else
                    {
                        Tree(r.A, (soFar ?? string.Empty) + "0");
                        Tree(r.B, (soFar ?? string.Empty) + "1");
                    }
                }
                Tree(root, string.Empty);
                Queue<string> encodeds = new Queue<string>();
                foreach (char c in text)
                {
                    encodeds.Enqueue(encodingDictionary[c]);
                }

                StringBuilder working = new StringBuilder(string.Empty);
                Queue<char> charsToAdd = new Queue<char>();

                int spares = 0;
                while (encodeds.Count > 0)
                {
                    while (working.Length < 16)
                    {
                        if (encodeds.Count > 0)
                        {
                            working.Append(encodeds.Dequeue());
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (working.Length >= 16)
                    {
                        //result.Append((16 - working.Length) + "$D");
                        //working.Append(Enumerable.Repeat('0', 16 - working.Length).ToArray());
                        //once we're here, working has AT LEAST 16 bits.
                        string toChar = working.ToString().Substring(0, 16);//chop off the first 16 bits of the working.
                        working.Remove(0, 16); //in conjunction with line above.
                        charsToAdd.Enqueue((char)Convert.ToInt32(toChar, 2)); //takes the new char and adds it to chars to add.
                    }
                    else
                    {
                        //this is the last iteration
                        if (working.Length > 0)
                        {
                            spares = (16 - working.Length);
                            working.Append(Enumerable.Repeat('0', 16 - working.Length).ToArray());
                            string toChar = working.ToString().Substring(0, 16);//chop off the first 16 bits of the working.
                            working.Remove(0, 16); //in conjunction with line above.
                            charsToAdd.Enqueue((char)Convert.ToInt32(toChar, 2)); //takes the new char and adds it to chars to add.
                        }
                    }
                }
                result.Append((char)(spares + 60) + "$D");

                int expectedLength = charsToAdd.Count + 5 + root.Serialize().Length;
                foreach (char c in charsToAdd)
                {
                    result.Append(c);
                }
                //result looks like : "[serialized tree]$D[number of whitespace zeros]$D[data]"
                return result.ToString();
            }
            internal string Decode(string encoded)
            {
                StringBuilder result = new StringBuilder(string.Empty);
                StringBuilder tree = new StringBuilder(encoded.Substring(0, encoded.IndexOf("$D")));
                root = new DualNode(tree.ToString());//$D#$D[DATA]
                int firstD = encoded.IndexOf("$D");
                int secondD = encoded.IndexOf("$D", firstD + 1);
                uint whitespace = ((uint)encoded[firstD + 2]) - 60;
                string data = encoded.Substring(firstD + 5);
                Queue<string> process = new Queue<string>();
                for (int i = 0; i < data.Length; i++)
                {
                    if (i == data.Length - 1)
                    {
#warning I think something is wrong here.
                        string s = Convert.ToString(((short)data[i]), 2);
                        s = s.PadLeft(16, '0');
                        s = s.Substring(0, ((int)s.Length - (int)whitespace));
                        process.Enqueue(s);
                    }
                    else
                    {
                        // does 1 (dec) cast as "1" in short or "0001000000000001"
                        string temp = Convert.ToString(((short)data[i]), 2);
                        temp = temp.PadLeft(16, '0');
                        process.Enqueue(temp);
                    }
                }
                int expectedLength = process.Count + 5 + root.Serialize().Length;
                StringBuilder working = new StringBuilder(string.Empty);
                DualNode localRoot = root;
                do
                {
                    if (working.Length == 0)
                    {
                        working.Append(process.Dequeue());
                    }
                    char c = working[0];
                    working.Remove(0, 1);
                    switch (c)
                    {
                        case '0':
                            if (localRoot.A.IsLeaf)
                            {
                                result.Append(localRoot.A.character.Value);
                                localRoot = root;
                            }
                            else
                            {
                                localRoot = localRoot.A;
                            }
                            break;
                        case '1':
                            if (localRoot.B.IsLeaf)
                            {
                                result.Append(localRoot.B.character.Value);
                                localRoot = root;
                            }
                            else
                            {
                                localRoot = localRoot.B;
                            }
                            break;
                        default:
                            throw new Exception("Error: Uh-oh. This exception shouldn't be possible to hit. Talk to Sam");
                    }
                } while (process.Count > 0 || working.Length > 0);
                return result.ToString();
            }
        }
        #endregion

        #region Fields
        #region Consts
        public static readonly Regex yes = new Regex("^[y|Ye|Es|S]|[y|Y]");
        public static readonly Regex no = new Regex("^[n|No|O]|[n|N]");
        #endregion

        #region Class Fields
        public readonly static HashAlgorithm sha;
        public readonly static Random rand;
        #endregion
        #endregion

        static Grand()
        {
            sha = new SHA1CryptoServiceProvider();
            rand = new Random(((DateTime.Now.Millisecond - 500) * 17) / 5);
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
        }
        public static string HuffmanSerialize<T>(T toSerialize) where T : ISerializable
        {
            throw new NotImplementedException();
        }
        public static string HuffmanDeserialize(string huff)
        {
            return new HuffmanCoder().Decode(huff);
        }
        public static T HuffmanDeserialize<T>(string huff) where T : ISerializable
        {
            throw new NotImplementedException();
        }
    }
}