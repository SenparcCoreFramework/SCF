using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Senparc.Scf.Core.Utility;
using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Core.Validator
{
    public class ValidatorContainer<T>
    {
        //private List<string> _errorList;

        public T ValidatorObject { get; set; }
        public string ValueName { get; set; }
        public bool Stopped { get; set; }
        public bool IsValid { get; set; }
        public string HtmlName { get; set; }
        public ModelStateDictionary ModelState { get; set; }
        public IValidatorEnvironment ValidatorEnvironment { get; set; }

        //public List<string> ErrorList
        //{
        //    get { return _errorList; }
        //    set
        //    {
        //        _errorList = value;
        //        this.IsValid = false;
        //    }
        //}

        public ValidatorContainer(IValidatorEnvironment validatorEnvironment, T validatorEnvironmentObject, string valueName, string htmlName)
        {
            ValidatorObject = validatorEnvironmentObject;
            ValueName = valueName;
            HtmlName = htmlName;
            ModelState = validatorEnvironment.ModelState;
            ValidatorEnvironment = validatorEnvironment;
            IsValid = true;
            //ErrorList = new List<string>();
        }

        /// <summary>
        /// 此方法暂时不能自动让ModelState.IsValid变为false
        /// </summary>
        /// <param name="errorMessage"></param>
        public void AddError(string errorMessage)
        {
            this.ModelState.AddModelError(this.HtmlName, errorMessage);
            this.IsValid = false;
        }
    }

    public static class ValidatorExtension
    {
        public static ValidatorContainer<T> Validator<T>(this IValidatorEnvironment validatorEnvironment, T validatorEnvironmentObject, string valueName, string htmlName)
        {
            return Validator<T>(validatorEnvironment, validatorEnvironmentObject, valueName, htmlName, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatorEnvironment"></param>
        /// <param name="validatorEnvironmentObject"></param>
        /// <param name="valueName"></param>
        /// <param name="htmlName"></param>
        /// <param name="nullOrEmptyable">是否允许为null或为空。输入null默认为不判断。首尾连续空格将被过滤（.Trim()）</param>
        /// <returns></returns>
        public static ValidatorContainer<T> Validator<T>(this IValidatorEnvironment validatorEnvironment, T validatorEnvironmentObject, string valueName, string htmlName, bool? nullOrEmptyable = null)
        {
            if (validatorEnvironment.ModelState[htmlName] != null
                && validatorEnvironment.ModelState[htmlName].Errors.Count > 0)
            {
                //移除原有的ModelState中的"The value '' is invalid."错误
                //TODO:可以使用Resource自行配置
                validatorEnvironment.ModelState[htmlName].Errors.Clear();
            }
            if (nullOrEmptyable != null && (validatorEnvironmentObject == null || string.IsNullOrEmpty(validatorEnvironmentObject.ToString().Trim())))
            {
                if (nullOrEmptyable == true)//可为空
                {
                    return null;//为空，返回，但是不记录错误
                }
                else//不可为空
                {
                    var container = new ValidatorContainer<T>(validatorEnvironment, validatorEnvironmentObject, valueName, htmlName);
                    return container.NotNullOrEmpty(true);
                }
            }
            else
            {
                return new ValidatorContainer<T>(validatorEnvironment, validatorEnvironmentObject, valueName, htmlName);//不判断是否为空
            }
        }


        public static ValidatorContainer<T> NotNullOrEmpty<T>(this ValidatorContainer<T> container)
        {
            return NotNullOrEmpty(container, true);
        }

        public static ValidatorContainer<T> NotNullOrEmpty<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (container.ValidatorObject == null || string.IsNullOrEmpty(container.ValidatorObject.ToString().Trim()))
            {
                string errorMessage = string.Format("提示： {0}!", container.ValueName);
                //container.ErrorList.Add(errorMessage);
                container.AddError(errorMessage);

                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }


        public static ValidatorContainer<T> IsTrue<T>(this ValidatorContainer<T> container, Func<T, bool> func, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (!func.Invoke(container.ValidatorObject))
            {
                string errorMessage = string.Format(failMessageFormat, container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }

            return container;
        }

        public static ValidatorContainer<T> IsFalse<T>(this ValidatorContainer<T> container, Func<T, bool> func, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (func.Invoke(container.ValidatorObject))
            {
                string errorMessage = string.Format(failMessageFormat, container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }

            return container;
        }

        /// <summary>
        /// 确认发生错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="failMessageFormat"></param>
        /// <param name="stopWhileFail"></param>
        /// <returns></returns>
        public static ValidatorContainer<T> Fail<T>(this ValidatorContainer<T> container, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            string errorMessage = string.Format(failMessageFormat, container.ValueName);
            container.AddError(errorMessage);
            if (stopWhileFail)
            {
                return null;
            }

            return container;
        }

        public static ValidatorContainer<T> Regex<T>(this ValidatorContainer<T> container, string expression, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            Regex emailExpression = new Regex(expression, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (!emailExpression.IsMatch((container.ValidatorObject.ToString())))
            {
                string errorMessage = string.Format(failMessageFormat, container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="stopWhileFail"></param>
        /// <returns></returns>
        public static ValidatorContainer<T> IsSafeSqlString<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            Regex emailExpression = new Regex(@"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (emailExpression.IsMatch((container.ValidatorObject.ToString())))
            {
                string errorMessage = string.Format("{0}中存在非法字符！", container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="stopWhileFail"></param>
        /// <returns></returns>
        public static ValidatorContainer<T> IsSafeUserInfoString<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            Regex emailExpression = new Regex(@"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|管理员|^Guest|admin", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (emailExpression.IsMatch((container.ValidatorObject.ToString())))
            {
                string errorMessage = string.Format("{0}中存在非法字符！", container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        public static ValidatorContainer<T> IsValidateUserInfoName<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }
            string userName = container.ValidatorObject.ToString();
            if (userName.IndexOf("　") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1)
            {
                container.AddError("用户名中不允许包含全角空格符");
                if (stopWhileFail)
                {
                    return null;
                }
            }
            if (userName.IndexOf(" ") != -1)
            {
                container.AddError("用户名中不允许包含空格");
                if (stopWhileFail)
                {
                    return null;
                }
            }
            if (userName.IndexOf(":") != -1)
            {
                container.AddError("用户名中不允许包含冒号");
                if (stopWhileFail)
                {
                    return null;
                }
            }

            string invalidateUserName = "`~!@#$%^&*()+-=;':\",./<>?|\\";//TODO:可以用正则判断
            foreach (var item in invalidateUserName)
            {
                if (userName.Contains(item))
                {
                    container.AddError("用户名中可以使用中文、英文或数字及下划线_，但不允许包含特殊符号：" + invalidateUserName);
                    if (stopWhileFail)
                    {
                        return null;
                    }
                }
            }

            return container;
        }

        public static ValidatorContainer<T> IsEmail<T>(this ValidatorContainer<T> container)
        {
            return IsEmail(container, true);
        }
        public static ValidatorContainer<T> IsEmail<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            return Regex(container,
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                "请在{0}中填写正确的Email地址！", stopWhileFail);
        }

        public static ValidatorContainer<T> IsMobile<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            //return Regex(container, @"^1[358]\d{9}$", "请在{0}中填写正确的电话号码！", stopWhileFail);
            //return Regex(container, @"^1[358]\d{9}$", "请填写正确的电话号码！", stopWhileFail);

            //电信手机号码正则        string dianxin = @"^1[3578][01379]\d{8}$";        Regex dReg = new Regex(dianxin);        //联通手机号正则        string liantong = @"^1[34578][01256]\d{8}$";        Regex tReg = new Regex(liantong);        //移动手机号正则        string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";        Regex yReg = new Regex(yidong);

            return Regex(container, @"^(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$", "请填写正确的电话号码！", stopWhileFail);
        }

        public static ValidatorContainer<T> IsIPAddress<T>(this ValidatorContainer<T> container)
        {
            return IsIPAddress(container, true);
        }

        public static ValidatorContainer<T> IsIPAddress<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            return Regex<T>(container,
                @"(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])",
                "请在{0}中填写正确的IP地址！", stopWhileFail);
        }

        public static ValidatorContainer<T> IsAvailableUrl<T>(this ValidatorContainer<T> container, bool stopWhileFail)
        {
            return Regex(container, @"HTTP(S)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请在{0}中填写正确的Url地址！", stopWhileFail);
        }


        public static ValidatorContainer<T> MaxByte<T>(this ValidatorContainer<T> container, int maxByte)
        {
            return MaxByte(container, maxByte, true);
        }
        public static ValidatorContainer<T> MaxByte<T>(this ValidatorContainer<T> container, int maxByte, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (System.Text.Encoding.Default.GetByteCount(container.ValidatorObject.ToString()) > maxByte)
            {
                string errorMessage = string.Format("{0}中最多只能输入{1}字节的内容（对应{2}汉字）", container.ValueName, maxByte, Convert.ToInt32(maxByte / 2));
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        public static ValidatorContainer<T> MaxLength<T>(this ValidatorContainer<T> container, int maxLength)
        {
            return MaxLength(container, maxLength, true);
        }
        public static ValidatorContainer<T> MaxLength<T>(this ValidatorContainer<T> container, int maxLength, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (container.ValidatorObject.ToString().Length > maxLength)
            {
                string errorMessage = string.Format("{0}中最多只能输入{1}个字符", container.ValueName, maxLength);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }


        public static ValidatorContainer<T> MinByte<T>(this ValidatorContainer<T> container, int minByte)
        {
            return MinByte(container, minByte, true);
        }
        public static ValidatorContainer<T> MinByte<T>(this ValidatorContainer<T> container, int minByte, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (System.Text.Encoding.Default.GetByteCount(container.ValidatorObject.ToString()) < minByte)
            {
                string errorMessage = string.Format("{0}中至少需要入{1}字节的内容（对应{2}汉字）", container.ValueName, minByte, Convert.ToInt32(minByte / 2));
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }


        public static ValidatorContainer<T> MinLength<T>(this ValidatorContainer<T> container, int minLength)
        {
            return MinLength(container, minLength, true);
        }
        public static ValidatorContainer<T> MinLength<T>(this ValidatorContainer<T> container, int minLength, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (container.ValidatorObject.ToString().Length < minLength)
            {
                string errorMessage = string.Format("{0}中至少需要输入{1}个字符", container.ValueName, minLength);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }


        public static ValidatorContainer<T> IsEqual<T>(this ValidatorContainer<T> container, T obj, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (!container.ValidatorObject.Equals(obj))
            {
                string errorMessage = string.Format(failMessageFormat, container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        public static ValidatorContainer<T> IsNotEqual<T>(this ValidatorContainer<T> container, T obj, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (container.ValidatorObject.Equals(obj))
            {
                string errorMessage = string.Format(failMessageFormat, container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        public static ValidatorContainer<T> Exclude<T>(this ValidatorContainer<T> container, string[] excludeStrings)
        {
            return Exclude(container, excludeStrings, true);
        }
        public static ValidatorContainer<T> Exclude<T>(this ValidatorContainer<T> container, string[] excludeStrings, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            foreach (var str in excludeStrings)
            {
                if (container.ValidatorObject.ToString().Contains(str))
                {
                    string charts = string.Join(",", excludeStrings).Replace(" ", "空格").Replace("　", "全角空格");
                    string errorMessage = string.Format("{0}中包含了不允许使用的特殊字符：{1}", container.ValueName, str);
                    container.AddError(errorMessage);
                    if (stopWhileFail)
                    {
                        return null;
                    }
                }
                return container;
            }
            return container;
        }

        public static ValidatorContainer<T> IsNumber<T>(this ValidatorContainer<T> container, float? min, float? max, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }
            min = min ?? float.MinValue;
            max = max ?? float.MaxValue;
            float tryFloat;
            if (!float.TryParse(container.ValidatorObject.ToString(), out tryFloat) || tryFloat < (float)min || tryFloat > (float)max)
            {
                string errorMessage = string.Format("请输入正确的{0}", container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        /// <summary>
        /// 校验验证码，建议Validator构造函数中的nullOrEmptyable为false
        /// </summary>
        /// <param name="container"></param>
        /// <param name="checkCodeKind"></param>
        /// <param name="stopWhileFail"></param>
        /// <returns></returns>
        public static ValidatorContainer<T> ValidateCheckCode<T>(this ValidatorContainer<T> container, CheckCodeKind checkCodeKind, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (container.ValidatorObject == null)
            {
                container.AddError("请输入验证码");
                if (stopWhileFail)
                {
                    return null;
                }
            }

            CheckCodeHandle checkCodeHandle = new CheckCodeHandle(checkCodeKind, container.ValidatorEnvironment.HttpContext);
            if (!checkCodeHandle.ValidateCheckCode(container.ValidatorObject.ToString()))
            {
                container.AddError("请输入正确的验证码");
                if (stopWhileFail)
                {
                    return null;
                }
            }

            return container;
        }


        public static ValidatorContainer<T> IsNull<T>(this ValidatorContainer<T> container, object obj, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (obj != null)
            {
                string errorMessage = string.Format(failMessageFormat, container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }

        public static ValidatorContainer<T> IsNotNull<T>(this ValidatorContainer<T> container, object obj, string failMessageFormat, bool stopWhileFail)
        {
            if (container == null)
            {
                return null;
            }

            if (obj == null)
            {
                string errorMessage = string.Format(failMessageFormat, container.ValueName);
                container.AddError(errorMessage);
                if (stopWhileFail)
                {
                    return null;
                }
            }
            return container;
        }
    }
}
