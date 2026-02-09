using Microsoft.EntityFrameworkCore;
using API_UP2.DTOs;
using API_UP2.Context;

namespace API_UP2.Services
{
    public class StudentService
    {
        private readonly StudentManagementContext _context;

        public StudentService(StudentManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetFilteredStudentsAsync(FilterDTO filter)
        {
            var query = _context.Students
                .Include(s => s.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(s =>
                    s.LastName.Contains(filter.Search) ||
                    s.FirstName.Contains(filter.Search) ||
                    s.Name.Contains(filter.Search));
            }

            if (int.TryParse(filter.DepartmentId, out int departmentId))
            {
                query = query.Where(s => s.DepartmentId == departmentId);
            }

            if (!string.IsNullOrEmpty(filter.GroupName))
            {
                query = query.Where(s => s.Education_Group == filter.GroupName);
            }

            if (!string.IsNullOrEmpty(filter.Funding))
            {
                query = query.Where(s => s.Financy == filter.Funding);
            }

            if (filter.IsExpelled.HasValue)
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.Information_Deduction) == filter.IsExpelled.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByStatusAsync(string statusType, bool activeOnly = true, DateTime? filterDate = null)
        {
            var query = _context.Students
                .Include(s => s.Department)
                .AsQueryable();

            var checkDate = filterDate ?? DateTime.Now;

            switch (statusType.ToLower())
            {
                case "orphan":
                    var orphanIds = await _context.Orphans
                        .Where(o => !activeOnly || (o.StartStatus <= checkDate && (o.EndStatus == null || o.EndStatus > checkDate)))
                        .Select(o => o.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => orphanIds.Contains(s.Id));
                    break;

                case "disabled":
                    var disabledIds = await _context.StudentDisabledPeople
                        .Where(d => !activeOnly || (d.StartStatus <= checkDate && (d.EndStatus == null || d.EndStatus > checkDate)))
                        .Select(d => d.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => disabledIds.Contains(s.Id));
                    break;

                case "ovz":
                    var ovzIds = await _context.StudentOVZ
                        .Where(o => !activeOnly || (o.StartStatus <= checkDate && (o.EndStatus == null || o.EndStatus > checkDate)))
                        .Select(o => o.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => ovzIds.Contains(s.Id));
                    break;

                case "svo":
                    var svoIds = await _context.StudentSVO
                        .Where(s => !activeOnly || (s.StartStatus <= checkDate && (s.EndStatus == null || s.EndStatus > checkDate)))
                        .Select(s => s.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => svoIds.Contains(s.Id));
                    break;

                case "sop":
                    var sopIds = await _context.StudentSOP
                        .Where(s => !activeOnly || (s.DateDelivery <= checkDate && (s.DateDeRegistration == null || s.DateDeRegistration > checkDate)))
                        .Select(s => s.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => sopIds.Contains(s.Id));
                    break;

                case "social":
                    var socialIds = await _context.SocialPayout
                        .Where(sp => !activeOnly || (sp.StartStatus <= checkDate && (sp.EndStatus == null || sp.EndStatus > checkDate)))
                        .Select(sp => sp.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => socialIds.Contains(s.Id));
                    break;

                case "hostel":
                    var hostelIds = await _context.StudentHostel
                        .Where(h => !activeOnly || (h.CheckInDate <= checkDate && (h.EvictionDate == null || h.EvictionDate > checkDate)))
                        .Select(h => h.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => hostelIds.Contains(s.Id));
                    break;

                case "sppp":
                    var spppIds = await _context.StudentSPPP
                        .Where(s => s.DateSppp.Year == checkDate.Year)
                        .Select(s => s.StudentId)
                        .Distinct()
                        .ToListAsync();
                    query = query.Where(s => spppIds.Contains(s.Id));
                    break;

                default:
                    throw new ArgumentException($"Неизвестный тип статуса: {statusType}");
            }

            return await query.ToListAsync();
        }

        public async Task<Dictionary<string, object>> GetStudentFullInfoAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                return null;

            var result = new Dictionary<string, object>
            {
                ["student"] = student
            };

            // Получаем все статусы студента
            var orphanStatuses = await _context.Orphans
                .Where(o => o.StudentId == studentId)
                .ToListAsync();
            if (orphanStatuses.Any()) result["orphans"] = orphanStatuses;

            var disabledStatuses = await _context.StudentDisabledPeople
                .Where(d => d.StudentId == studentId)
                .Include(d => d.Student)
                .ToListAsync();
            if (disabledStatuses.Any()) result["disabled"] = disabledStatuses;

            var ovzStatuses = await _context.StudentOVZ
                .Where(o => o.StudentId == studentId)
                .Include(o => o.Student)
                .ToListAsync();
            if (ovzStatuses.Any()) result["ovz"] = ovzStatuses;

            var svoStatuses = await _context.StudentSVO
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Student)
                .ToListAsync();
            if (svoStatuses.Any()) result["svo"] = svoStatuses;

            var sopStatuses = await _context.StudentSOP
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Student)
                .ToListAsync();
            if (sopStatuses.Any()) result["sop"] = sopStatuses;

            var socialStatuses = await _context.SocialPayout
                .Where(sp => sp.StudentId == studentId)
                .Include(sp => sp.Student)
                .ToListAsync();
            if (socialStatuses.Any()) result["social"] = socialStatuses;

            var hostelStatuses = await _context.StudentHostel
                .Where(h => h.StudentId == studentId)
                .Include(h => h.Student)
                .ToListAsync();
            if (hostelStatuses.Any()) result["hostel"] = hostelStatuses;

            var spppMeetings = await _context.StudentSPPP
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Student)
                .OrderByDescending(s => s.DateSppp)
                .ToListAsync();
            if (spppMeetings.Any()) result["sppp"] = spppMeetings;

            return result;
        }

        public async Task<List<Student>> GetStudentsWithAllStatusesAsync(DateTime? date = null)
        {
            var checkDate = date ?? DateTime.Now;

            var students = await _context.Students
                .Include(s => s.Department)
                .ToListAsync();

            var result = new List<Student>();

            foreach (var student in students)
            {
                bool hasActiveStatus = false;

                // Проверяем каждый статус
                hasActiveStatus |= await _context.Orphans
                    .AnyAsync(o => o.StudentId == student.Id &&
                                 o.StartStatus <= checkDate &&
                                 (o.EndStatus == null || o.EndStatus > checkDate));

                hasActiveStatus |= await _context.StudentDisabledPeople
                    .AnyAsync(d => d.StudentId == student.Id &&
                                 d.StartStatus <= checkDate &&
                                 (d.EndStatus == null || d.EndStatus > checkDate));

                hasActiveStatus |= await _context.StudentOVZ
                    .AnyAsync(o => o.StudentId == student.Id &&
                                 o.StartStatus <= checkDate &&
                                 (o.EndStatus == null || o.EndStatus > checkDate));

                hasActiveStatus |= await _context.StudentSVO
                    .AnyAsync(s => s.StudentId == student.Id &&
                                 s.StartStatus <= checkDate &&
                                 (s.EndStatus == null || s.EndStatus > checkDate));

                hasActiveStatus |= await _context.StudentSOP
                    .AnyAsync(s => s.StudentId == student.Id &&
                                 s.DateDelivery <= checkDate &&
                                 (s.DateDeRegistration == null || s.DateDeRegistration > checkDate));

                hasActiveStatus |= await _context.SocialPayout
                    .AnyAsync(sp => sp.StudentId == student.Id &&
                                  sp.StartStatus <= checkDate &&
                                  (sp.EndStatus == null || sp.EndStatus > checkDate));

                hasActiveStatus |= await _context.StudentHostel
                    .AnyAsync(h => h.StudentId == student.Id &&
                                 h.CheckInDate <= checkDate &&
                                 (h.EvictionDate == null || h.EvictionDate > checkDate));

                if (hasActiveStatus)
                {
                    result.Add(student);
                }
            }

            return result;
        }

        public async Task<Dictionary<string, int>> GetStatisticsAsync()
        {
            var today = DateTime.Today;

            var statistics = new Dictionary<string, int>
            {
                ["totalStudents"] = await _context.Students.CountAsync(),
                ["expelledStudents"] = await _context.Students
                    .CountAsync(s => !string.IsNullOrEmpty(s.Information_Deduction)),

                ["activeOrphans"] = await _context.Orphans
                    .CountAsync(o => o.StartStatus <= today && (o.EndStatus == null || o.EndStatus > today)),

                ["activeDisabled"] = await _context.StudentDisabledPeople
                    .CountAsync(d => d.StartStatus <= today && (d.EndStatus == null || d.EndStatus > today)),

                ["activeOVZ"] = await _context.StudentOVZ
                    .CountAsync(o => o.StartStatus <= today && (o.EndStatus == null || o.EndStatus > today)),

                ["activeSVO"] = await _context.StudentSVO
                    .CountAsync(s => s.StartStatus <= today && (s.EndStatus == null || s.EndStatus > today)),

                ["activeSOP"] = await _context.StudentSOP
                    .CountAsync(s => s.DateDelivery <= today && (s.DateDeRegistration == null || s.DateDeRegistration > today)),

                ["activeSocial"] = await _context.SocialPayout
                    .CountAsync(sp => sp.StartStatus <= today && (sp.EndStatus == null || sp.EndStatus > today)),

                ["inHostel"] = await _context.StudentHostel
                    .CountAsync(h => h.CheckInDate <= today && (h.EvictionDate == null || h.EvictionDate > today)),

                ["spppThisYear"] = await _context.StudentSPPP
                    .CountAsync(s => s.DateSppp.Year == today.Year)
            };

            return statistics;
        }
    }
}