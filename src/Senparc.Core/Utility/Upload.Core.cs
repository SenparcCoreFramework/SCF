using Microsoft.AspNetCore.Http;
using Senparc.CO2NET.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Senparc.Core.Utility
{
    public partial class Upload
    {
        #region 私有（内部）方法

        /// <summary>
        /// 上传图片（使用默认允许扩展名：".gif", ".png", ".bmp", ".jpg"）
        /// </summary>
        /// <param name="saveOnServerPath">保存到服务器路径（"~/upload/"下面）</param>
        /// <param name="file">HttpPostedFileBase</param>
        /// <param name="fileNameOnServer">保存文件名（不包含扩展名）</param>
        /// <param name="limit">限制大小（KB）</param>
        /// <param name="isDel">是否删除已存在</param>
        /// <param name="allowedExtension">允许上传的扩展名</param>
        /// <returns></returns>
        private static string UploadFile_Img(string saveOnServerPath, IFormFile file, string fileNameOnServer, long limit, bool isDel)
        {
            return UploadFile_Img(saveOnServerPath, file, fileNameOnServer, limit, isDel, null);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="saveOnServerPath">保存到服务器路径（"~/upload/"下面）</param>
        /// <param name="file">HttpPostedFileBase</param>
        /// <param name="fileNameOnServer">保存文件名（不包含扩展名）</param>
        /// <param name="limit">限制大小（KB）</param>
        /// <param name="isDel">是否删除已存在</param>
        /// <returns></returns>
        private static string UploadFile_Img(string saveOnServerPath, IFormFile file, string fileNameOnServer, long limit, bool isDel, string[] allowedExtension)
        {
            allowedExtension = allowedExtension ?? new string[] { ".gif", ".png", ".bmp", ".jpg" };//允许扩展名
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            //long limit = 22000000;//限制大小（KB）

            //bool fileOK = false;
            //则判断文件类型是否符合要求
            if (allowedExtension.Contains(fileExtension))
            {
                //fileOK = true;

                //限制大小  220M  ——by TNT2
                if (file.Length< limit * 1024)
                {
                    if (!saveOnServerPath.EndsWith("/"))
                    {
                        saveOnServerPath += "/";
                    }
                    return UploadFile("~/upload/" + saveOnServerPath, fileNameOnServer, file, limit, isDel);
                }
                else
                {
                    return "只能上传" + limit + "KB以内的文件！此文件大小：" + file.Length / 1024 + " KB";
                }
            }
            else
            {
                return "只能上传图片文件！";
            }
        }

        /// <summary>
        /// 私有方法，上传文件，防止外部调用
        /// </summary>
        /// <param name="savePathStr"></param>
        /// <param name="file">HttpPostedFileBase</param>
        /// <param name="fileNameOnServer">保存文件名（不包含扩展名）</param>
        /// <param name="limit">限制大小（KB）</param>
        /// <param name="isDel">是否删除已存在</param>
        /// <returns></returns>
        private static string UploadFile(string savePathStr, string fileNameOnServer, IFormFile file, long limit, bool isDel, string[] allowedExtension = null)
        {
            //上传    ——by TNT2;
            string saveFileName = string.Empty;//服务器上地址,如果成功，返回正确地址


            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            if (allowedExtension != null && !allowedExtension.Contains(fileExtension))
            {
                return $"为确保系统安全，此文件类型（{fileExtension}）被禁止上传，如确实需要上传，请联系客服。";
            }

            //调用SaveAs方法，实现上传，并显示相关信息    ——by TNT2

            //总限制大小  220M  ——by TNT2
            if (file.Length < limit * 1024)
            {
                //实现上传    ——by TNT2
                //获取给予运用程序根文件夹的绝对路径    ——by TNT2
                saveFileName = fileNameOnServer + Path.GetExtension(file.FileName).ToLower();//保存文件名
                string savePhyicalPath = SenparcHttpContext.MapPath(savePathStr);
                string savePhyicalFilePath = Path.Combine(savePhyicalPath, saveFileName);//服务器上的保存物理路径
                string saveApplicationPath = Path.Combine(GetFullApplicationPathFromVirtualPath(savePathStr), saveFileName);//文件的网站绝对地址

                //保存文件
                if (isDel && File.Exists(savePhyicalFilePath))
                {
                    File.Delete(savePhyicalFilePath);//先删除
                }

                if (!Directory.Exists(savePhyicalPath))
                {
                    Directory.CreateDirectory(savePhyicalPath);
                }

                using (var reader = new FileStream(savePhyicalFilePath, FileMode.OpenOrCreate))
                {
                    file.CopyTo(reader);
                    reader.Flush();
                }

                //返回图片的网站绝对地址
                return saveApplicationPath;
            }
            else
            {
                return "只能上传" + limit + "KB以内的文件！此文件大小：" + file.Length / 1024 + " KB";
            }

            //return saveFileName;
        }

        /// <summary>
        /// 从应用程序虚拟路径，获取应用程序路径（相对网站根目录，/开头）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFullApplicationPathFromVirtualPath(string virtualPath)
        {
            //return virtualPath.Replace("~/", HttpContext.Current.Request.ApplicationPath);
            return virtualPath.Replace("~/", "/");//COCONET 这里暂时默认所有应用程序根目录都是/
        }
        /// <summary>
        /// 从应用程序路径，获取应用程序虚拟路径（~/开头）
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetVirtualPathFormFullApplicationPath(string appPath)
        {
            if (appPath.IsNullOrEmpty())
            {
                return "";
            }
            //return "~/" + appPath.Substring(HttpContext.Current.Request.ApplicationPath.Length, appPath.Length - 1);

            return "~/" + appPath.Substring("/".Length, appPath.Length - 1);//COCONET 这里暂时默认所有应用程序根目录都是/
        }

        /// <summary>
        /// 获取缩略图信息，返回缩略图物理路径和源文件物理路径
        /// </summary>
        /// <param name="uploadResult">通过UploadFile()方法返回的结果（如果上传成功，返回相对网站根目录路径）</param>
        /// <param name="fileFormat">缩略文件格式（在Config.UpLoadFileFormat中）</param>
        /// <param name="filePhyicalPath">源文件物理路径</param>
        /// <returns></returns>
        private static string GetTumbnailFileName(string uploadResult, string fileFormat, out string filePhyicalPath)
        {
            string fileApplicationPath = GetVirtualPathFormFullApplicationPath(uploadResult);//还原ApplicationPath
            filePhyicalPath = SenparcHttpContext.MapPath(fileApplicationPath);//源文件物理路经
            string thumbnailFilename = Path.GetDirectoryName(filePhyicalPath)//和原始图片放于同一文件夹
                      + "/" + string.Format(fileFormat, Path.GetFileName(uploadResult), Path.GetExtension(uploadResult));//原文件名+标记+原扩展名
            return thumbnailFilename;
        }

        #endregion

        /*  以上为私有方法  */
        

        /// <summary>
        /// 察看上传是否成功
        /// </summary>
        /// <param name="saveFileName">上传返回的字符串，如果“/”开头，则表示上传成功，不然里面包含的是错误信息</param>
        /// <returns></returns>
        public static bool CheckUploadSuccessful(string saveFileName)
        {
            if (saveFileName != null && saveFileName.StartsWith("/"))
                return true;
            else
                return false;
        }

        /*  
        扩展示例

          //public static string UpLoadUserImage(HttpPostedFileBase file)
        //{
        //    string filename = file.FileName;
        //    string newFilename = DateTime.Now.Ticks.ToString();
        //    string[] allowedExtension = new string[] { ".jpg", ".gif", ".png", ".bmp" };
        //    string uploadResult = UploadFile_Img("WeixinUser", file, newFilename, 2400, true, allowedExtension);//执行上传

        //    //制作缩略图
        //    if (CheckUploadSuccessful(uploadResult))
        //    {
        //        string bigSourceFile = null;
        //        //大幅缩略图
        //        int width = 180;
        //        int height = 180;//缩略图宽、高
        //        string filePhyicalPath = string.Empty;//源文件物理路经
        //        string thumbnailFilename = GetTumbnailFileName(uploadResult, Senparc.Core.Config.UpLoadFileFormat.PRODUCT_PCICURE_Big, out filePhyicalPath);
        //        bigSourceFile = thumbnailFilename;
        //        ImageUtility.ImageHelper.GetThumbnail(filePhyicalPath, width, height, false, thumbnailFilename, false);//保存缩略图

        //        //中幅缩略图
        //        width = 50;
        //        height = 50;//缩略图宽、高
        //        filePhyicalPath = string.Empty;//源文件物理路经
        //        thumbnailFilename = GetTumbnailFileName(uploadResult, Senparc.Core.Config.UpLoadFileFormat.PRODUCT_PCICURE_MIDDLE, out filePhyicalPath);
        //        ImageUtility.ImageHelper.GetThumbnail(filePhyicalPath, width, height, false, thumbnailFilename, false);//保存缩略图

        //        //小幅缩略图
        //        width = 30;
        //        height = 30;//缩略图宽、高
        //        filePhyicalPath = string.Empty;//源文件物理路经
        //        thumbnailFilename = GetTumbnailFileName(uploadResult, Senparc.Core.Config.UpLoadFileFormat.PRODUCT_PCICURE_SMALL, out filePhyicalPath);
        //        ImageUtility.ImageHelper.GetThumbnail(filePhyicalPath, width, height, false, thumbnailFilename, true);//保存缩略图

        //        //修改文件的名称
        //        ImageUtility.ImageHelper.UpdateFileName(bigSourceFile, uploadResult);
        //    }
        //    return uploadResult;
        //}

            */
    }
}
