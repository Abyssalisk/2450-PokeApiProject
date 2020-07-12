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
using System.Collections;
using System.Collections.Specialized;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    public static partial class Grand
    {
        #region Typedefs
        #region Non-Generic
        /// <summary>
        /// Author: Samuel Gardner<br/><br/>
        /// </summary>
        /// <remarks>
        /// The HuffmanCoder is a wrapper for a compression algorithm, effective at compressing larger strings that consist of as few unique characters as possible. 
        /// (json and xml are good candidates for this), If the provided string fills a large percentage of the source character set, say 80%, this compression algorithms output
        /// is likely to be larger than the original object. This algorithm compresses context-independant; i.e., the size of the resulting object should be hypothetically 
        /// identical regardless of the order of the characters in the original string.
        /// </remarks>
        public class HuffmanCoder
        {
            #region Typedefs
            /// <summary>
            /// DualNode is used to make the decryption/encryption tree for Huffman coding.
            /// </summary>
#warning If there's time, implement object pooling on this.
            private protected class DualNode
            {
                #region Fields
                #region Instance Fields
                /// <summary>
                /// Sum of the number values in the A and B nodes, if this node is a leaf, 
                /// it is the number of instances of the character that occured in the string.
                /// </summary>
                public uint numerical = 0;
                /// <summary>
                /// The character of this leaf, or null if this is a branch.
                /// </summary>
                public char? character = null;
                #endregion

                #region Properties
                /// <summary>
                /// Whether or not this node is a leaf (It's a leaf if it has no child nodes, and has a character)
                /// </summary>
                public bool IsLeaf
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
                /// The parent of this node. (No longer necessary in current system).
                /// </summary>
                //public DualNode Parent { get; set; } = null;
                /// <summary>
                /// Property "A" storage field
                /// </summary>
                private DualNode a;
                /// <summary>
                /// The 1st child of this node.
                /// </summary>
                public DualNode A
                {
                    get => a;
                    set
                    {
                        //if (value != null)
                        //{
                        //    value.Parent = this;
                        //}
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
                public DualNode B
                {
                    get => b;
                    set
                    {
                        //if (value != null)
                        //{
                        //    value.Parent = this;
                        //}
                        b = value;
                    }
                }
                #endregion
                #endregion

                #region Constructors
                /// <summary>
                /// Used internally with object initializers.
                /// </summary>
                public DualNode()
                {

                }
                /// <summary>
                /// Creates a DualNode object (including its subtree) provided in the serialized 
                /// information.
                /// </summary>
                /// <param name="serialized">The serialized data. 
                /// This is assumed to be in proper form.</param>
                public DualNode(string serialized)
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
                    //cheaty way of fixing the bug when reaching the end of this branch.
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
                public string Serialize()
                {
                    return $"[N:{this.numerical}C:" +
                        $"{((this.character == null) ? "NL" : this.character.Value.ToString())}" +
                        $"A:{(this.A == null ? "NL" : A.Serialize())}" +
                        $"B:{(this.B == null ? "NL" : B.Serialize())}]";
                }
                #endregion

                #region Class Methods
#warning Make this non static and have it mutate this instance's tree (maybe perhaps)
                /// <summary>
                /// Deserializes a DualNode tree and returns the root.
                /// </summary>
                /// <param name="serialized">The serialized DualNode tree</param>
                /// <returns>The root of the deserialized node tree</returns>
                public static DualNode Deserialize(string serialized)
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
#if DEBUG
                /// <summary>
                /// Takes a serialized DualNode tree structure and formats it with indents and newlines to make it easier to read.
                /// </summary>
                /// <param name="serialized">The serialized DualNode tree</param>
                /// <returns>The formatted serialized DualNode tree</returns>
                public static string FormatSerializedTree(string serialized)
                {
                    //holds the working value and is what is returned at the end.
                    StringBuilder result = new StringBuilder(serialized);
                    //Match to find an "A:[" or "B:[" that doesn't have a newline preceeding it. (the check for newline might be unecessary at this point, but it works, and I don't want to break it).
                    Regex beginBrace = new Regex("(?<=\\n{0})(A|B):\\[");
                    //sets our first search.
                    Match m = beginBrace.Match(serialized);
                    //Keeps track of how many '\t''s we are ahead of the left justification.
                    int indentCount = 0;
                    //Keeps track of the delta length between before and after adding this iterations formatting characters (if this iteration added 3 '\t''s and 1 '\n' then delta is 4)
                    int delta = 0;
                    //Keeps track of the running total of extra characters the result string has compared to the original (to offset the index when adding the formatting characters).
                    int running = 0;
                    //Keeps track of the index of the current iterations match index, this is used by the next iterations regex match by setting the search start index equal to this value plus 1.
                    int previousIndex = m.Index;
                    while (m.Success)
                    {
                        if (serialized[m.Index - 1] != ']' || (serialized[m.Index - 1] == ']' && serialized[m.Index - 2] == ':'))
                        {
                            indentCount++;
                            delta = result.Length;
                            result.Insert(m.Index + running, Enumerable.Repeat('\t', indentCount).ToArray());
                            result.Insert(m.Index + running, System.Environment.NewLine);
                            delta = result.Length - delta;
                            //adds the index offset.
                            running += delta;
                        }
                        else
                        {
                            int counter = -1;
                            int index = 1;
                            do
                            {
                                if (serialized[m.Index - index] == ']')
                                {
                                    counter++;
                                }
                                else
                                {
                                    break;
                                }
                                index++;
                            } while (true);
                            indentCount -= counter;
                            delta = result.Length;
                            result.Insert(m.Index + running, Enumerable.Repeat('\t', indentCount).ToArray());
                            result.Insert(m.Index + running, System.Environment.NewLine);
                            delta = result.Length - delta;
                            //adds the index offset.
                            running += delta;
                        }
                        //sets the next index to work on by searching for the first occurance after the previous index.
                        m = beginBrace.Match(serialized, previousIndex + 1);
                        //sets the previous index as this index, for use when the line above is run again next iteration.
                        previousIndex = m.Index;
                    }
                    return result.ToString();
                }
#endif
                #endregion
                #endregion
            }
            #endregion

            #region Fields
            #region Instance Fields
            /// <summary>
            /// The root node of this HuffmanCoder object.
            /// </summary>
            private protected DualNode root;
            #endregion

            //Yes I know Properties arent fields, but the storage for the properties are, and I don't think it makes sense to seperate them.
            #region Properties
            private protected string compressedObject;
            /// <summary>
            /// The compressed form of the object. If this value was not set manually, but the <see cref="DecompressedObject"/> property was set, this value will be generated, 
            /// cached, and returned. Otherwise, this will return <c>null</c>.
            /// </summary>
            public string CompressedObject
            {
                get
                {
                    if (compressedObject != null)
                    {
                        return compressedObject;
                    }
                    else if (decompressedObject != null)
                    {
                        compressedObject = Encode(DecompressedObject);
                        return compressedObject;
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    if (compressedObject != value)
                    {
                        compressedObject = value;
                        decompressedObject = null;
                    }
                }
            }
            private protected string decompressedObject;
            /// <summary>
            /// The decompressed form of the object. If this value was not set manually, but the <see cref="CompressedObject"/> property was set, this value will be generated, 
            /// cached, and returned. Otherwise, this will return <c>null</c>.
            /// </summary>
            public string DecompressedObject
            {
                get
                {
                    if (decompressedObject != null)
                    {
                        return decompressedObject;
                    }
                    else if (compressedObject != null)
                    {
                        decompressedObject = Decode(compressedObject);
                        return decompressedObject;
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    if (decompressedObject != value)
                    {
                        decompressedObject = value;
                        compressedObject = null;
                    }
                }
            }
            #endregion
            #endregion

            #region Methods
            /// <summary>
            /// Converts the serialized object into a <see cref="HuffmanCoder"/> compressed form.
            /// </summary>
            /// <param name="serialized">The serialized form of the object</param>
            /// <returns>The compressed form the of the object</returns>
            private protected string Encode(string serialized)
            {
                root = null;
                StringBuilder result = new StringBuilder(string.Empty);
                {
                    List<DualNode> dangling = new List<DualNode>();
                    #region Equivalency Block
                    #region Old
                    //Known to be working.
                    //{
                    //    IOrderedEnumerable<KeyValuePair<char, uint>> orderedCommonality;
                    //    {
                    //        Dictionary<char, uint> commonalityDict = new Dictionary<char, uint>();
                    //        foreach (char c in text)
                    //        {
                    //            if (commonalityDict.ContainsKey(c))
                    //            {
                    //                commonalityDict[c]++;
                    //            }
                    //            else
                    //            {
                    //                commonalityDict.Add(c, 1);
                    //            }
                    //        }
                    //        orderedCommonality = commonalityDict.OrderBy((x) => x.Value);
                    //    }
                    //    foreach (KeyValuePair<char, uint> v in orderedCommonality)
                    //    {
                    //        dangling.Enqueue(new DualNode()
                    //        {
                    //            A = null,
                    //            B = null,
                    //            character = v.Key,
                    //            numerical = v.Value
                    //        });
                    //    }
                    //}
                    #endregion
                    #region New
                    {
                        IOrderedEnumerable<KeyValuePair<char, uint>> orderedCommonality;
                        {
                            Dictionary<char, uint> commonalityDict = new Dictionary<char, uint>();
                            foreach (char c in serialized)
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
                        //reuse old nodes if present.
                        {
                            Queue<DualNode> oldNodes;
                            void DisassembleTree(DualNode focus)
                            {
                                oldNodes.Enqueue(focus);
                                if (!focus.IsLeaf)
                                {
                                    DisassembleTree(focus.A);
                                    DisassembleTree(focus.B);
                                }
                            }
                            if (root != null)
                            {
                                oldNodes = new Queue<DualNode>();
                                DisassembleTree(root);
                                IEnumerator<KeyValuePair<char, uint>> iter = orderedCommonality.GetEnumerator();
                                DualNode temp = null;
                                for (int i = 0; i < orderedCommonality.Count(); i++)
                                {
                                    if (oldNodes.Count > 0)
                                    {
                                        temp = oldNodes.Dequeue();
                                        KeyValuePair<char, uint> tempVal = iter.Current;
                                        iter.MoveNext();
                                        temp.character = tempVal.Key;
                                        temp.numerical = tempVal.Value;
                                        dangling.Add(temp);
                                        //dangling.Enqueue(temp);
                                    }
                                    else
                                    {
                                        KeyValuePair<char, uint> tempVal = iter.Current;
                                        iter.MoveNext();
                                        temp = new DualNode()
                                        {
                                            character = tempVal.Key,
                                            numerical = tempVal.Value
                                        };
                                        dangling.Add(temp);
                                        //dangling.Enqueue(temp);
                                    }
                                }
                                root = null;
                            }
                            else
                            {
                                dangling = new List<DualNode>(orderedCommonality.Select(x => new DualNode() { A = null, B = null, character = x.Key, numerical = x.Value }));
                            }
                        }
                    }
                    #endregion
                    #endregion
                    //For use in the sort method call below.
                    int Compare(DualNode a, DualNode b)
                    {
                        return (int)a.numerical - (int)b.numerical;
                    }
                    do
                    {
                        if (dangling.Count == 1)
                        {
                            //root = dangling.Dequeue();
                            root = dangling[0]; //break and set root once the tree is done being built.
                            dangling.RemoveAt(0);
                            break;
                        }
                        DualNode temp = dangling[0]; //get first
                        dangling.RemoveAt(0); //remove first so obj init can peek the second.
                        dangling.Add(new DualNode()
                        {
                            //A = dangling.Peek(),
                            //numerical = dangling.Dequeue().numerical + dangling.Peek().numerical,
                            //B = dangling.Dequeue(),
                            A = temp, //set first
                            numerical = temp.numerical + dangling[0].numerical, //set num of first plus num of peek
                            B = dangling[0], //set peek
                            character = null //this isn't a char because all chars were set above.
                        });
                        dangling.RemoveAt(0); //remove peek
                        //dangling = new Queue<DualNode>(dangling.OrderBy(x => x.numerical));
                        dangling.Sort(Compare); //resort list.
                    } while (true); //this will always break, (so dont worry).
                }
                //string s = Grand.HuffmanCoder.DualNode.FormatSerializedTree(root.Serialize());

                //if (root?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A?.A != null)
                //{
                //    throw new Exception("Error: One or more characters could be encoded as a \'\\0\'.");
                //}
                #region Equivalency Block
                #region Old
                //result.Append(root.Serialize() + "$D");
                //Dictionary<char, string> encodingDictionary = new Dictionary<char, string>();
                //void Tree(DualNode r, string soFar)
                //{
                //    if (r.IsLeaf)
                //    {
                //        encodingDictionary.Add(r.character.Value, soFar);//new string(soFar.ToCharArray().Reverse().ToArray())
                //    }
                //    else
                //    {
                //        Tree(r.A, (soFar ?? string.Empty) + "0");
                //        Tree(r.B, (soFar ?? string.Empty) + "1");
                //    }
                //}
                //Tree(root, string.Empty);
                //Queue<string> encodeds = new Queue<string>();
                //foreach (char c in serialized)
                //{
                //    encodeds.Enqueue(encodingDictionary[c]);
                //}
                #endregion
                #region New
                result.Append(root.Serialize() + "$D");
                Dictionary<char, ValueTuple<BitVector32, byte>> encodingDictionary = new Dictionary<char, ValueTuple<BitVector32, byte>>();
                void Tree(DualNode r, BitVector32 soFar, byte len)
                {
                    if (r.IsLeaf)
                    {
                        encodingDictionary.Add(r.character.Value, new ValueTuple<BitVector32, byte>(soFar, len));//new string(soFar.ToCharArray().Reverse().ToArray())
                    }
                    else
                    {
                        BitVector32 pass = new BitVector32(soFar.Data << 1);
                        Tree(r.A, pass, (byte)(len + 1));
                        pass = new BitVector32(pass.Data | 1);
                        Tree(r.B, pass, (byte)(len + 1));
                    }
                }
                Tree(root, new BitVector32(0), 0);
                Queue<string> encodeds = new Queue<string>();
                foreach (char c in serialized)
                {
                    encodeds.Enqueue(Convert.ToString(encodingDictionary[c].Item1.Data, 2).PadLeft(encodingDictionary[c].Item2, '0'));
                }
                #endregion
                #endregion

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
                    while (working.Length >= 16)
                    {
                        string toChar = working.ToString().Substring(0, 16);//chop off the first 16 bits of the working.
                        working.Remove(0, 16); //in conjunction with line above.
                        charsToAdd.Enqueue((char)Convert.ToInt32(toChar, 2)); //takes the new char and adds it to chars to add.
                    }
                }
                if (working.Length > 0)
                {
                    spares = (16 - working.Length);
                    //working.Append(Enumerable.Repeat('0', spares).ToArray());
                    //string toChar = working.ToString().Substring(0, 16);//chop off the first 16 bits of the working.
                    //working.Remove(0, 16); //in conjunction with line above.
                    string toChar = working.ToString().PadRight(16, '0');
                    charsToAdd.Enqueue((char)Convert.ToInt32(toChar, 2)); //takes the new char and adds it to chars to add.
                }
                result.Append(((char)(spares + 60)) + "$D");

                //int expectedLength = charsToAdd.Count + 5 + root.Serialize().Length;
                foreach (char c in charsToAdd)
                {
                    result.Append(c);
                }
                //result looks like : "[serialized tree]$D[number of whitespace zeros]$D[data]"
                return result.ToString();
            }
            /// <summary>
            /// Converts the compressed <see cref="HuffmanCoder"/> object into its decompressed form.
            /// </summary>
            /// <param name="compressed">The compressed form of the object</param>
            /// <returns>The decompressed form the of the object</returns>
            private protected string Decode(string encoded)
            {
                StringBuilder result = new StringBuilder(string.Empty);
                StringBuilder tree = new StringBuilder(encoded.Substring(0, encoded.IndexOf("$D")));
                root = new DualNode(tree.ToString());//$D#$D[DATA]
                int firstD = encoded.IndexOf("$D");
                int secondD = encoded.IndexOf("$D", firstD + 1);
                uint whitespace = ((uint)encoded[firstD + 2]) - 60;
                string data = encoded.Substring(firstD + 5);
//#warning ended optimization here.
                Queue<string> process = new Queue<string>();
                for (int i = 0; i < data.Length; i++)
                {
                    if (i == data.Length - 1)
                    {
                        string s = Convert.ToString(((short)data[i]), 2);
                        s = s.PadLeft(16, '0');
                        s = s.Substring(0, ((int)s.Length - (int)whitespace));
                        process.Enqueue(s);
                    }
                    else
                    {
                        // does 1 (dec) cast as "1" in short or "0000000000000001" (former is true)
                        string temp = Convert.ToString(((short)data[i]), 2);
                        temp = temp.PadLeft(16, '0');
                        process.Enqueue(temp);
                    }
                }
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
            #endregion
        }
        #endregion

        #region Generic
        /// <summary>
        /// Author: Samuel Gardner<br/><br/>
        /// </summary>
        /// <remarks>
        /// The HuffmanCoder is a wrapper for a compression algorithm, effective at compressing larger strings that consist of as few unique characters as possible. 
        /// (json and xml are good candidates for this), If the provided string fills a large percentage of the source character set, say 80%, this compression algorithms output
        /// is likely to be larger than the original object. This algorithm compresses context-independant; i.e., the size of the resulting object should be hypothetically 
        /// identical regardless of the order of the characters in the original string.
        /// </remarks>
        public class HuffmanCoder<T> : HuffmanCoder where T : class
        {
            #region New Properties
            /// <summary>
            /// The compressed form of the object. If this value was not set manually, but the <see cref="DecompressedObject"/> property was set, this value will be generated, 
            /// cached, and returned. Otherwise, this will return <c>null</c>.
            /// </summary>
            public new string CompressedObject
            {
                get
                {
                    if (compressedObject != null)
                    {
                        return compressedObject;
                    }
                    else if (decompressedObject != null)
                    {
                        compressedObject = Encode(JsonConvert.SerializeObject(decompressedObject));
                        return compressedObject;
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    compressedObject = value;
                    decompressedObject = null;
                }
            }
            public new T decompressedObject;
            /// <summary>
            /// The decompressed form of the object. If this value was not set manually, but the <see cref="CompressedObject"/> property was set, this value will be generated, 
            /// cached, and returned. Otherwise, this will return <c>null</c>.
            /// </summary>
            public new T DecompressedObject
            {
                get
                {
                    if (decompressedObject != null)
                    {
                        return decompressedObject;
                    }
                    else if (compressedObject != null)
                    {
                        decompressedObject = JsonConvert.DeserializeObject<T>(Decode(compressedObject));
                        return decompressedObject;
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    decompressedObject = value;
                    compressedObject = null;
                }
            }
            #endregion
        }
        #endregion
        #endregion
    }
}