// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: sample.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Bird {

  /// <summary>Holder for reflection information generated from sample.proto</summary>
  public static partial class SampleReflection {

    #region Descriptor
    /// <summary>File descriptor for sample.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SampleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgxzYW1wbGUucHJvdG8SBGJpcmQiJgoERmVlZBILCgNzZXEYASABKAUSEQoJ",
            "Zm9vZF9uYW1lGAIgASgJIkAKCEZlZWRiYWNrEgsKA3NlcRgBIAEoBRIPCgdj",
            "b21tZW50GAIgASgJEhYKDmVjaG9fZm9vZF9uYW1lGAMgASgJYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Bird.Feed), global::Bird.Feed.Parser, new[]{ "Seq", "FoodName" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Bird.Feedback), global::Bird.Feedback.Parser, new[]{ "Seq", "Comment", "EchoFoodName" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Feed : pb::IMessage<Feed> {
    private static readonly pb::MessageParser<Feed> _parser = new pb::MessageParser<Feed>(() => new Feed());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Feed> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Bird.SampleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Feed() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Feed(Feed other) : this() {
      seq_ = other.seq_;
      foodName_ = other.foodName_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Feed Clone() {
      return new Feed(this);
    }

    /// <summary>Field number for the "seq" field.</summary>
    public const int SeqFieldNumber = 1;
    private int seq_;
    /// <summary>
    /// id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Seq {
      get { return seq_; }
      set {
        seq_ = value;
      }
    }

    /// <summary>Field number for the "food_name" field.</summary>
    public const int FoodNameFieldNumber = 2;
    private string foodName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string FoodName {
      get { return foodName_; }
      set {
        foodName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Feed);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Feed other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Seq != other.Seq) return false;
      if (FoodName != other.FoodName) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Seq != 0) hash ^= Seq.GetHashCode();
      if (FoodName.Length != 0) hash ^= FoodName.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Seq != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Seq);
      }
      if (FoodName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(FoodName);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Seq != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Seq);
      }
      if (FoodName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(FoodName);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Feed other) {
      if (other == null) {
        return;
      }
      if (other.Seq != 0) {
        Seq = other.Seq;
      }
      if (other.FoodName.Length != 0) {
        FoodName = other.FoodName;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Seq = input.ReadInt32();
            break;
          }
          case 18: {
            FoodName = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Feedback : pb::IMessage<Feedback> {
    private static readonly pb::MessageParser<Feedback> _parser = new pb::MessageParser<Feedback>(() => new Feedback());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Feedback> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Bird.SampleReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Feedback() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Feedback(Feedback other) : this() {
      seq_ = other.seq_;
      comment_ = other.comment_;
      echoFoodName_ = other.echoFoodName_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Feedback Clone() {
      return new Feedback(this);
    }

    /// <summary>Field number for the "seq" field.</summary>
    public const int SeqFieldNumber = 1;
    private int seq_;
    /// <summary>
    /// id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Seq {
      get { return seq_; }
      set {
        seq_ = value;
      }
    }

    /// <summary>Field number for the "comment" field.</summary>
    public const int CommentFieldNumber = 2;
    private string comment_ = "";
    /// <summary>
    /// say something about the food
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Comment {
      get { return comment_; }
      set {
        comment_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "echo_food_name" field.</summary>
    public const int EchoFoodNameFieldNumber = 3;
    private string echoFoodName_ = "";
    /// <summary>
    /// return the received food name
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string EchoFoodName {
      get { return echoFoodName_; }
      set {
        echoFoodName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Feedback);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Feedback other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Seq != other.Seq) return false;
      if (Comment != other.Comment) return false;
      if (EchoFoodName != other.EchoFoodName) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Seq != 0) hash ^= Seq.GetHashCode();
      if (Comment.Length != 0) hash ^= Comment.GetHashCode();
      if (EchoFoodName.Length != 0) hash ^= EchoFoodName.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Seq != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Seq);
      }
      if (Comment.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Comment);
      }
      if (EchoFoodName.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(EchoFoodName);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Seq != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Seq);
      }
      if (Comment.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Comment);
      }
      if (EchoFoodName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(EchoFoodName);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Feedback other) {
      if (other == null) {
        return;
      }
      if (other.Seq != 0) {
        Seq = other.Seq;
      }
      if (other.Comment.Length != 0) {
        Comment = other.Comment;
      }
      if (other.EchoFoodName.Length != 0) {
        EchoFoodName = other.EchoFoodName;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Seq = input.ReadInt32();
            break;
          }
          case 18: {
            Comment = input.ReadString();
            break;
          }
          case 26: {
            EchoFoodName = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
