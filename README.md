<h1 align="center">Online Shop API</h1>

<p dir="rtl" align="center">
یک API فروشگاه آنلاین با ASP.NET Core و معماری تمیز
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10-purple" />
  <img src="https://img.shields.io/badge/Architecture-Clean-blue" />
  <img src="https://img.shields.io/badge/Auth-JWT-green" />
  <img src="https://img.shields.io/badge/Auth-Identity-green" />
  <img src="https://img.shields.io/badge/Validation-FluentValidation-orange" />
  <img src="https://img.shields.io/badge/EFCore-blue" />
  <img src="https://img.shields.io/badge/Pagination-yellow" />
</p>

<hr/>

<h2>🚀 Features | امکانات</h2>

<ul>
<li><b>Authentication</b> => ثبت نام، ورود، تغییر رمز عبور، بازیابی رمز عبور و مدیریت نقش ها</li>
<li><b>Authorization</b> => نقش های مدیر، فروشنده و مشتری</li>
<li><b>Product Management</b> => افزودن، دریافت، ویرایش و حذف نرم محصولات</li>
<li><b>Category Management</b> => افزودن، دریافت، ویرایش و حذف نرم دسته بندی ها</li>
<li><b>Shopping Cart</b> => افزودن، ویرایش، حذف و پاکسازی آیتم های سبد خرید</li>
<li><b>Order Management</b> => ایجاد سفارش از یک یا تمام آیتم های سبد خرید</li>
<li><b>Payment</b> => ایجاد و تایید پرداخت از طریق درگاه زرین پال</li>
<li><b>Stock Management</b> => کاهش و بازگردانی موجودی هنگام پرداخت یا لغو سفارش</li>
<li><b>Pagination and Sorting</b> => صفحه بندی و مرتب سازی فهرست محصولات، سفارش ها، پرداخت ها و سبدهای خرید</li>
<li><b>Validation</b> =>FluentValidation اعتبارسنجی اطلاعات ورودی با</li>
<li><b>Global Exception Handling</b> => مدیریت متمرکز خطاهای پیش بینی نشده</li>
<li><b>Logging</b> =>در کنسول و فایل Serilog ثبت رویدادها و خطاها با </li>
<li><b>API Documentation</b> => Swagger با API و نمایش XML مستندسازی کد با توضیحات </li>
</ul>

<h2>🧱 Architecture | معماری</h2>

<p>پروژه بر اساس معماری تمیز و جداسازی مسئولیت ها ساخته شده است</p>

<ul>
<li><b>Domain</b> => موجودیت ها، وضعیت ها و مدل های مشترک</li>
<li><b>Application</b> =>(AutoMapper)ها، اعتبارسنجی و نگاشت داده هاDTO ،سرویس ها، رابط ها،</li>
<li><b>Infrastructure</b> =>و پایگاه داده Entity Framework Core، هاRepository، Unit of Work، ساخت توکن، درگاه پرداخت</li>
<li><b>OnlineShop.Api</b> => کنترلرها، میان افزارها، تنظیمات برنامه و نقطه ورود API</li>
</ul>

<h2>🛠 Technologies | فناوری ها</h2>

<ul>
<li>ASP.NET Core و .NET 10</li>
<li>Entity Framework Core و SQL Server</li>
<li>ASP.NET Core Identity</li>
<li>JWT Bearer And Identity Authentication</li>
<li>FluentValidation</li>
<li>AutoMapper</li>
<li>Serilog</li>
<li>Swagger و OpenAPI</li>
<li>درگاه پرداخت زرین پال</li>
</ul>

<h2>▶️ Run the project | اجرای پروژه</h2>

<ol>
<li>مخزن را دریافت کنید</li>
<li>رشته اتصال پایگاه داده را در تنظیمات برنامه قرار دهید</li>
<li>تنظیمات احراز هویت و درگاه پرداخت را کامل کنید</li>
<li>Migrationهای پایگاه داده را اعمال کنید</li>
<li>پروژه را اجرا کنید</li>
</ol>

```bash
cd OnlineShop
dotnet restore
dotnet build
dotnet run --project OnlineShop.Api
```

پس از اجرا، صفحه Swagger از طریق نشانی پیش فرض برنامه در دسترس خواهد بود

<h2>⚙️ Configuration | تنظیمات</h2>

تنظیمات حساس مانند رشته اتصال پایگاه داده، کلید محرمانه احراز هویت و اطلاعات پذیرنده درگاه را در فایل عمومی پروژه قرار ندهید و برای محیط واقعی از متغیرهای محیطی یا مدیریت امن اسرار استفاده کنید

<h2>🔐 Main API Areas | بخش های اصلی API</h2>

<ul>
<li><code>/api/auth</code> => احراز هویت و مدیریت نقش کاربران</li>
<li><code>/api/product</code> => مدیریت محصولات</li>
<li><code>/api/category</code> => مدیریت دسته بندی ها</li>
<li><code>/api/cart</code> => مدیریت سبد خرید</li>
<li><code>/api/order</code> => مدیریت سفارش ها و پرداخت ها</li>
</ul>

<h2>💳 Payment Flow | روند پرداخت</h2>

<ol>
<li>مشتری سفارش ایجاد میکند</li>
<li>برای سفارش درخواست پرداخت ایجاد میشود</li>
<li>مشتری به درگاه پرداخت منتقل میشود</li>
<li>نتیجه پرداخت به نشانی بازگشت ارسال میشود</li>
<li>پرداخت از طریق درگاه بررسی و تایید میشود</li>
<li>وضعیت پرداخت، سفارش و موجودی محصولات به روز میشود</li>
</ol>

<h2>📝 Documentation</h2>

توضیحات XML برای کلاس ها، متدها و مدل های مهم پروژه نوشته شده اند و میتوانند برای تولید مستندات فنی و نمایش اطلاعات API در Swagger استفاده شوند