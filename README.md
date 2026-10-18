# מערכת לניהול והשאלת ציוד (Equipment Loan System)

מערכת Web API ב-.NET 8 המנהלת ציוד והשאלות בזמן אמת, עם טיפול בתחרות משאבים מקבילה (Optimistic Concurrency), בסיס נתונים SQL Server וממשק משתמש ב-React.

## 📌 סקירת המערכת
המערכת מנהלת עותקים פיזיים של ציוד (`ItemCopy`) ומאפשרת למשתמשים לבצע השאלות.
הפרויקט בנוי בארבע שכבות: `Core`, `Data`, `Service`, ו-`API`.

## ⚡ תחרות על משאב מוגבל (Optimistic Concurrency)
המשאב המוגבל במערכת הוא העותק הפיזי של הציוד (`ItemCopy`).
מיושם מנגנון **Optimistic Concurrency**:
1. לכל ישות `ItemCopy` מוגדר שדה גרסה (`Version`).
2. בזמן עדכון, EF Core בודק תאימות לגרסה בבסיס הנתונים.
3. במקרה של שינוי מקביל, נזרקת חריגת `DbUpdateConcurrencyException`.
4. השרת תופס את החריגה ומחזיר תשובת **`HTTP 409 Conflict`** עם הודעה ברורה למשתמש.

## 🔐 אימות והרשאות (JWT Authentication)
המערכת כוללת מנגנון אימות ב-JWT Token עם תפקידים:
* **`Admin`**: admin@example.com (סיסמה: 123456)
* **`User`**: user@example.com (סיסמה: 123456)

## 🧪 בדיקות אוטומטיות (Unit Tests)
בדיקות יחידה ב-xUnit עם Moq בפרויקט `EquipmentLoanSystem.Tests` המכסות תרחישי הצלחה ותחרות משאבים (409 Conflict).

## 🚀 הוראות הרצה
1. הרצת השרת: `dotnet run --project EquipmentLoanSystem.API`
2. הרצת הלקוח: `cd equipment-loan-client` ולאחר מכן `npm start`