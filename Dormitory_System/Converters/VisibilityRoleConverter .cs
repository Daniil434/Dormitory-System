using Dormitory_System.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Dormitory_System.Converters
{
    public class VisibilityRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is User user && parameter is string requiredRole)
            {
                // Проверяем, совпадает ли роль пользователя с требуемой
                return user.RoleName == requiredRole ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed; // По умолчанию скрываем
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
