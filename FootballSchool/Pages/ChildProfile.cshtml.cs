using System.ComponentModel.DataAnnotations;
using FootballSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FootballSchool.Pages
{
    public class ChildProfileModel : PageModel
    {
        private readonly FootballSchoolContext _context;

        public ChildProfileModel(FootballSchoolContext context)
        {
            _context = context;
        }

        public IList<Student> Children { get; private set; } = new List<Student>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }

        [BindProperty]
        public ChildInputModel CreateChild { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? EditId { get; set; }

        [BindProperty]
        public ChildEditInputModel? EditChild { get; set; }

        public async Task OnGetAsync()
        {
            await LoadChildrenAsync();

            if (EditId.HasValue)
            {
                var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentId == EditId.Value);
                if (student is not null)
                {
                    EditChild = new ChildEditInputModel
                    {
                        StudentId = student.StudentId,
                        SurnameStudent = student.SurnameStudent,
                        NameStudent = student.NameStudent,
                        BirthStudent = student.BirthStudent,
                        GenderStudent = student.GenderStudent,
                        ParentNumber = student.ParentNumber,
                        LevelStudent = student.LevelStudent,
                        PhoneStudent = student.PhoneStudent,
                        SurnameParent = student.SurnameParent,
                        NameParent = student.NameParent,
                        CityStudent = student.CityStudent,
                        StreetStudent = student.StreetStudent,
                        HouseStudent = student.HouseStudent,
                        ApartmentStudent = student.ApartmentStudent
                    };
                }
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadChildrenAsync();
                return Page();
            }

            var student = new Student
            {
                TeamId = null,
                SurnameStudent = CreateChild.SurnameStudent,
                NameStudent = CreateChild.NameStudent,
                MiddleStudent = null,
                BirthStudent = CreateChild.BirthStudent,
                GenderStudent = CreateChild.GenderStudent,
                PhoneStudent = CreateChild.PhoneStudent,
                EmailStudent = null,
                MedicalStudent = null,
                LevelStudent = CreateChild.LevelStudent,
                PhotoStudent = null,
                ParentNumber = CreateChild.ParentNumber,
                SurnameParent = CreateChild.SurnameParent,
                NameParent = CreateChild.NameParent,
                MiddleParent = null,
                CityStudent = CreateChild.CityStudent,
                StreetStudent = CreateChild.StreetStudent,
                HouseStudent = CreateChild.HouseStudent,
                ApartmentStudent = CreateChild.ApartmentStudent
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            StatusMessage = "Профиль ребенка создан.";
            return RedirectToPage(new { search = Search });
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (EditChild is null)
            {
                return RedirectToPage(new { search = Search });
            }

            if (!ModelState.IsValid)
            {
                await LoadChildrenAsync();
                return Page();
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == EditChild.StudentId);
            if (student is null)
            {
                return NotFound();
            }

            student.SurnameStudent = EditChild.SurnameStudent;
            student.NameStudent = EditChild.NameStudent;
            student.BirthStudent = EditChild.BirthStudent;
            student.GenderStudent = EditChild.GenderStudent;
            student.ParentNumber = EditChild.ParentNumber;
            student.LevelStudent = EditChild.LevelStudent;
            student.PhoneStudent = EditChild.PhoneStudent;
            student.SurnameParent = EditChild.SurnameParent;
            student.NameParent = EditChild.NameParent;
            student.CityStudent = EditChild.CityStudent;
            student.StreetStudent = EditChild.StreetStudent;
            student.HouseStudent = EditChild.HouseStudent;
            student.ApartmentStudent = EditChild.ApartmentStudent;

            await _context.SaveChangesAsync();

            StatusMessage = "Профиль ребенка обновлен.";
            return RedirectToPage(new { search = Search });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
            if (student is null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            StatusMessage = "Профиль ребенка удален.";
            return RedirectToPage(new { search = Search });
        }

        private async Task LoadChildrenAsync()
        {
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var term = Search.Trim();
                query = query.Where(s =>
                    s.SurnameStudent.Contains(term) ||
                    s.NameStudent.Contains(term) ||
                    s.ParentNumber.Contains(term) ||
                    s.SurnameParent.Contains(term) ||
                    s.NameParent.Contains(term));
            }

            Children = await query
                .OrderBy(s => s.SurnameStudent)
                .ThenBy(s => s.NameStudent)
                .ToListAsync();
        }

        public class ChildInputModel
        {
            [Display(Name = "Фамилия ребенка")]
            [Required]
            public string SurnameStudent { get; set; } = string.Empty;

            [Display(Name = "Имя ребенка")]
            [Required]
            public string NameStudent { get; set; } = string.Empty;

            [Display(Name = "Дата рождения")]
            [Required]
            public DateOnly BirthStudent { get; set; }

            [Display(Name = "Пол")]
            [Required]
            public string GenderStudent { get; set; } = string.Empty;

            [Display(Name = "Телефон родителя")]
            [Required]
            public string ParentNumber { get; set; } = string.Empty;

            [Display(Name = "Уровень")]
            [Required]
            public string LevelStudent { get; set; } = string.Empty;

            [Display(Name = "Телефон ребенка")]
            public string? PhoneStudent { get; set; }

            [Display(Name = "Фамилия родителя")]
            [Required]
            public string SurnameParent { get; set; } = string.Empty;

            [Display(Name = "Имя родителя")]
            [Required]
            public string NameParent { get; set; } = string.Empty;

            [Display(Name = "Город")]
            [Required]
            public string CityStudent { get; set; } = string.Empty;

            [Display(Name = "Улица")]
            [Required]
            public string StreetStudent { get; set; } = string.Empty;

            [Display(Name = "Дом")]
            [Required]
            public string HouseStudent { get; set; } = string.Empty;

            [Display(Name = "Квартира")]
            public string? ApartmentStudent { get; set; }
        }

        public class ChildEditInputModel : ChildInputModel
        {
            public int StudentId { get; set; }
        }
    }
}
