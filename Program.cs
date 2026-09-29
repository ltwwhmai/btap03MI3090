/****************/
// 202418940
//Lê Thanh Mai
/****************/

using System;
using System.Text;

class Program
{
    static void Thu(Action hanh_dong)
    {
        try { hanh_dong(); }
        catch (Exception loi) { Console.WriteLine("  Lỗi bắt được: " + loi.Message); }
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("1. Tạo 2 Employee");
        var nv1 = new Employee("E01", "Le Thanh Mai");
        var nv2 = new Employee("E02", "Tran Thi B", 1000);

        Console.WriteLine("2. Tạo 2 SoftwareEngineer");
        var ky_su1 = new SoftwareEngineer("S01", "Nguyen Van C", "C#");
        var ky_su2 = new SoftwareEngineer("S02", "Pham Thi D", 2000, "Java", 300);

        Console.WriteLine("3. Tăng lương cố định");
        nv2.IncreaseSalary(500);
        Console.WriteLine("4. Tăng lương 10%");
        ky_su2.IncreaseSalary(10, true);

        Console.WriteLine("5. Tạo nhóm không trưởng nhóm");
        var nhom1 = new ProjectTeam("P01", "He thong ERP");

        Console.WriteLine("6. addMember(employee): " + nhom1.AddMember(nv2));
        Console.WriteLine("7. addMember(engineer, true): " + nhom1.AddMember(ky_su2, true));
        Console.WriteLine("8. Thêm trùng: " + nhom1.AddMember(nv2));

        Console.WriteLine("9. Hiển thị đa hình");
        nhom1.DisplayTeam();
        Console.WriteLine("10. Tổng chi phí: " + nhom1.CalculateTotalMonthlyCost().ToString("N0"));

        Console.WriteLine("11. Xóa trưởng nhóm hiện tại: " + nhom1.RemoveMember("S02"));
        Console.WriteLine("12. Đổi trưởng nhóm: " + nhom1.ChangeLeader(nv2));
        Console.WriteLine("    Xóa trưởng nhóm cũ: " + nhom1.RemoveMember("S02"));
        nhom1.DisplayTeam();

        Console.WriteLine("13-14. Nhóm 2 trong khối cục bộ, chứa nv2 (đã có ở nhóm 1)");
        using (var nhom2 = new ProjectTeam("P02", "Mobile App", ky_su1))
        {
            nhom2.AddMember(nv2);
            nhom2.DisplayTeam();
        } // nhom2.Dispose() được gọi ở đây

        Console.WriteLine("15. Nhân sự vẫn tồn tại sau khi nhóm 2 bị hủy:");
        nv2.DisplayInfo();
        ky_su1.DisplayInfo();
        Console.WriteLine("    nhom1 vẫn chứa E02: " + nhom1.Contains("E02"));

        Console.WriteLine("--- Trường hợp biên ---");
        Thu(() => new Employee("", "Ten"));
        Thu(() => new Employee("E09", "Ten", -1));
        Thu(() => nv1.IncreaseSalary(0));
        Thu(() => nv1.IncreaseSalary(-5, true));
        Thu(() => new SoftwareEngineer("S09", "Ten", ""));
        Thu(() => new SoftwareEngineer("S09", "Ten", 100, "C#", -1));
        Console.WriteLine("Xóa mã không tồn tại: " + nhom1.RemoveMember("XXX"));
        Console.WriteLine("Thêm null: " + nhom1.AddMember(null));
        Console.WriteLine("Constructor mặc định:");
        new Employee().DisplayInfo();
    }
}