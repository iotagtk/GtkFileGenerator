using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class clsEncodeFile {

  string filePath;
  string encodeName;
  StreamWriter sw;
  //StreamReader sr;
  //StringReader stri;

  Dictionary<string,int> enArray = new Dictionary<string,int>();

  public clsEncodeFile() {

     enArray["SHIFTJIS"] = 932;
     enArray["SHIFT_JIS"] = 932;
     enArray["SHIFT-JIS"] = 932;
     enArray["JIS"] = 50220;
     enArray["EUC"] = 51932;
     //enArray["UTF8"] = UTF8;
     //enArray["UTF-8"] = UTF8;
     //enArray["ASCII"] = ASCII;
  }

  public string _getEncode(string path){

    FileStream fs = new FileStream(path,System.IO.FileMode.Open,
    System.IO.FileAccess.Read);
    byte[] bs = new byte[fs.Length];
    fs.Read(bs,0,bs.Length);
    fs.Close();

    Encoding enc = GetCode(bs);
    encodeName = enc.GetEncoder().ToString();
    Console.WriteLine(encodeName);
    return encodeName;

 }

 public void _delete() {

     if(File.Exists(this.filePath) && this.filePath != "") {
        File.Delete(this.filePath);
     }

 }

 public string _read() {


     if(!File.Exists(filePath)) {
       return "";
     }

     FileStream fs = new FileStream(filePath,FileMode.Open,FileAccess.Read);
     byte[] bs = new byte[fs.Length];
     fs.Read(bs,0,bs.Length);
     fs.Close();

     System.Text.Encoding enc = GetCode(bs);

     if(enc==null) {
       enc = Encoding.UTF8;
     }

     var text = enc.GetString(bs);

     StringReader sr = new StringReader(text);

     string line;
     string str="";
     while((line = sr.ReadLine()) != null) {
       str += line + Environment.NewLine;
     }

     return str;

   
 
  }

 //文字コードを判定して指定したもに変更
 public void _save(string text) {



     if(encodeName==null) {
       byte[] bs = HexStringToBytes(text);
       encodeName = GetCode(bs).EncodingName;
     }

     if(!File.Exists(this.filePath) && this.filePath != "") {
       File.Create(this.filePath).Close();
     }

     sw = new StreamWriter(this.filePath,false,Encoding.GetEncoding(this.encodeName));
     sw.Write(text);

  
 }

 public string _convertEncoding(string str,string _iEncoding,string _oEncoding) {

     var _iEncoding1 = _iEncoding.ToUpper();
     var _oEncoding1 = _oEncoding.ToUpper();
     var encodeCode = 0;

     byte[] srcBytes = null;

     byte[] bs = HexStringToBytes(str);
     Encoding encodeName = GetCode(bs);

     if(encodeName == Encoding.GetEncoding(enArray[_oEncoding1])) {
       return str;
     }

    if(_iEncoding1 != "UTF-8" && _iEncoding1 != "ASCII") {
       encodeCode = enArray[_iEncoding1];
       srcBytes = Encoding.GetEncoding(encodeCode).GetBytes(str);

     } else if(_iEncoding1 == "UTF-8") {
       srcBytes = System.Text.Encoding.UTF8.GetBytes(str);

     } else if(_iEncoding1 == "ASCII") {
       srcBytes = System.Text.Encoding.ASCII.GetBytes(str);
     }       

     return Encoding.GetEncoding(enArray[_oEncoding1]).GetString(srcBytes);

  

 }

 private static byte[] HexStringToBytes(string byteString) {
   // 文字列の文字数(半角)が奇数の場合、頭に「0」を付ける
   int length = byteString.Length;
   if(length % 2 == 1) {
     byteString = "0" + byteString;
     length++;
   }

   List<byte> data = new List<byte>();

   for(int i=0;i<length-1;i=i+2) {
     // 16進数表記の文字列かどうかをチェック
     string buf = byteString.Substring(i,2);
     if(Regex.IsMatch(buf,@"^[0-9a-fA-F]{2}$")) {
       data.Add(Convert.ToByte(buf,16));
     }
     // 16進数表記で無ければ「00」とする
     else {
       data.Add(Convert.ToByte("00",16));
     }
   }

   return data.ToArray();
 }

  private static Encoding GetCode(byte[] byts)
  {
      const byte bESC = 0x1B;
      const byte bAT = 0x40;
      const byte bDollar = 0x24;
      const byte bAnd = 0x26;
      const byte bOP  = 0x28;
      const byte bB = 0x42;
      const byte bD = 0x44;
      const byte bJ = 0x4A;
      const byte bI = 0x49;

      int len = byts.Length;
      int binary = 0;
      int ucs2 = 0;
      int sjis = 0;
      int euc = 0;
      int utf8 = 0;
      byte b1, b2;

      for (int i = 0; i < len; i++)
      {
          if (byts[i] <= 0x06 || byts[i] == 0x7F || byts[i] == 0xFF)
          {
              //'binary'
              binary++;
              if (len - 1 > i && byts[i] == 0x00
                  && i > 0 && byts[i - 1] <= 0x7F)
              {
                  //smells like raw unicode
                  ucs2++;
              }
          }
      }

      if (binary > 0)
      {
          if (ucs2 > 0)
              //JIS
              //ucs2(Unicode)
              return System.Text.Encoding.Unicode;
          else
              //binary
              return null;
      }

      for (int i = 0; i < len - 1; i++)
      {
          b1 = byts[i];
          b2 = byts[i + 1];

          if (b1 == bESC)
          {
              if (b2 >= 0x80)
                  //not Japanese
                  //ASCII
                  return System.Text.Encoding.ASCII;
              else if (len - 2 > i &&
                  b2 == bDollar && byts[i + 2] == bAT)
                  //JIS_0208 1978
                  //JIS
                  return System.Text.Encoding.GetEncoding(50220);
              else if (len - 2 > i &&
                  b2 == bDollar && byts[i + 2] == bB)
                  //JIS_0208 1983
                  //JIS
                  return System.Text.Encoding.GetEncoding(50220);
              else if (len - 5 > i &&
                  b2 == bAnd && byts[i + 2] == bAT && byts[i + 3] == bESC &&
                  byts[i + 4] == bDollar && byts[i + 5] == bB)
                  //JIS_0208 1990
                  //JIS
                  return System.Text.Encoding.GetEncoding(50220);
              else if (len - 3 > i &&
                  b2 == bDollar && byts[i + 2] == bOP && byts[i + 3] == bD)
                  //JIS_0212
                  //JIS
                  return System.Text.Encoding.GetEncoding(50220);
              else if (len - 2 > i &&
                  b2 == bOP && (byts[i + 2] == bB || byts[i + 2] == bJ))
                  //JIS_ASC
                  //JIS
                  return System.Text.Encoding.GetEncoding(50220);
              else if (len - 2 > i &&
                  b2 == bOP && byts[i + 2] == bI)
                  //JIS_KANA
                  //JIS
                  return System.Text.Encoding.GetEncoding(50220);
          }
      }

      for (int i = 0; i < len - 1; i++)
      {
          b1 = byts[i];
          b2 = byts[i + 1];
          if (((b1 >= 0x81 && b1 <= 0x9F) || (b1 >= 0xE0 && b1 <= 0xFC)) &&
              ((b2 >= 0x40 && b2 <= 0x7E) || (b2 >= 0x80 && b2 <= 0xFC)))
          {
              sjis += 2;
              i++;
          }
      }
      for (int i = 0; i < len - 1; i++)
      {
          b1 = byts[i];
          b2 = byts[i + 1];
          if (((b1 >= 0xA1 && b1 <= 0xFE) && (b2 >= 0xA1 && b2 <= 0xFE)) ||
              (b1 == 0x8E && (b2 >= 0xA1 && b2 <= 0xDF)))
          {
              euc += 2;
              i++;
          }
          else if (len - 2 > i && 
              b1 == 0x8F && (b2 >= 0xA1 && b2 <= 0xFE) &&
              (byts[i + 2] >= 0xA1 && byts[i + 2] <= 0xFE))
          {
              euc += 3;
              i += 2;
          }
      }
      for (int i = 0; i < len - 1; i++)
      {
          b1 = byts[i];
          b2 = byts[i + 1];
          if ((b1 >= 0xC0 && b1 <= 0xDF) && (b2 >= 0x80 && b2 <= 0xBF))
          {
              utf8 += 2;
              i++;
          }
          else if (len - 2 > i &&
              (b1 >= 0xE0 && b1 <= 0xEF) && (b2 >= 0x80 && b2 <= 0xBF) &&
              (byts[i + 2] >= 0x80 && byts[i + 2] <= 0xBF))
          {
              utf8 += 3;
              i += 2;
          }
      }

      if (euc > sjis && euc > utf8)
          //EUC
          return Encoding.GetEncoding(51932);
      else if (sjis > euc && sjis > utf8)
          //SJIS
          return Encoding.GetEncoding(932);
      else if (utf8 > euc && utf8 > sjis)
          //UTF8
          return Encoding.UTF8;

      return null;
  }

  public string FilePath {
    set {
      this.filePath = value;
    }
    get {
      return this.filePath;
    }
  }

  public string EncodeName {
    get {
      return this.encodeName;
    }
    set {
      this.encodeName = value;
    }
  }


}

