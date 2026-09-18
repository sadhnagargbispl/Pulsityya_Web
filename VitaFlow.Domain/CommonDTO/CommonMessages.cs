using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.CommonDTO
{
    public static class CommonMessages  
    {
        // Token messages
        public const string TokenExpired = "The token is expired and not valid anymore.";
        public const string TokenUsernamePasswordWrong = "Incorrect username or password.";
        public const string UserDisabled = "User is disabled!";
        public const string TokenSignatureError = "Token signature not valid.";
        public const string TokenJsonError = "Unable to read JSON value.";
        public const string BlankToken = "Couldn't find authentication bearer, will ignore the header.";

        // User messages
        public const string UserNotFound = "User not found with given id or username and password.";
        public const string UserPasswordNotValid = "Entered Old password is not valid.";
        public const string UserPasswordChange = "Password changed successfully.";
        public const string UserIdNotFound = "User id not found.";
        public const string ErrorInGetUser = "Error in getting users.";
        public const string PasswordNotMatch = "Password not match.";
        public const string PasswordResetMsg = "A password reset message was sent to your email address. Please click the link to \"reset password\".";
        public const string OldPasswordNotMatch = "Old password not match.";
        public const string PasswordNotValid = "Password is not valid. Password should contain 6 to 20 characters string with at least one digit, one upper case letter, one lower case letter, and one special symbol (\"@#$%\").";
        public const string ErrorInRegistrationEmailSending = "Error while sending email at registration.";
        public const string ErrorInForgotPasswordEmailSending = "Error occurred while sending email at forgot password.";
        public const string ErrorInResetPasswordEmailSending = "Error occurred while sending email at reset password.";
        public const string UsernameDuplicateEntry = "User already exists with given email.";
        public const string UserDeleted = "User deleted successfully.";

        // Email messages
        public const string EmailRegistrationSubject = "Welcome to Koko Raffle Resort - Registration Successful!";
        public const string EmailVisitorRegistrationSubject = "Welcome to Koko Raffle Resort: Your User Registration Confirmation";
        public const string EmailForgotPasswordSubject = "Forgot Password";
        public const string EmailResetPasswordSubject = "Reset Password";

        // Setting messages
        public const string ErrorInGetSetting = "Error in getting settings.";
        public const string SettingIdNotFound = "Setting id not found.";
        public const string SettingNotFound = "Setting not found with given id.";

        // Employee messages
        public const string ErrorInGetEmployee = "Error in getting employee.";
        public const string EmployeeNotFound = "Cannot find employee with given id.";
        public const string EmployeeDeleted = "Employee deleted successfully.";

        // File messages
        public const string FileUploadSuccess = "File uploaded successfully.";

        // Company messages
        public const string ErrorInGetCompany = "Error in getting company.";
        public const string CompanyNotFound = "Cannot find company with given id.";
        public const string CompanyDeleted = "Company deleted successfully.";

        // Permissions messages
        public const string ErrorInGetPermissions = "Error in getting permissions.";
        public const string PermissionsNotFound = "Cannot find permissions with given id.";
        public const string PermissionsDeleted = "Permissions deleted successfully.";

        // Roles messages
        public const string ErrorInGetRoles = "Error in getting roles.";
        public const string RolesNotFound = "Cannot find roles with given id.";
        public const string RolesDeleted = "Roles deleted successfully.";

        // Visitors messages
        public const string ErrorInGetVisitors = "Error in getting visitors.";
        public const string VisitorsNotFound = "Cannot find visitors with given id.";
        public const string VisitorsDeleted = "Visitors deleted successfully.";

        // Visitor Member messages
        public const string ErrorInGetVisitorMember = "Error in getting visitor member.";
        public const string VisitorMemberNotFound = "Cannot find visitor member with given id.";
        public const string VisitorMemberDeleted = "Visitor member deleted successfully.";

        // Miscellaneous messages
        public const string ForgotPasswordEmailSendSuccessfully = "Email sent successfully.";
        public const string UserAddedSuccessfully = "Added user successfully.";
        public const string UserAddedError = "Error while saving user.";
        public const string UserEmailError = "Error while sending email to user!";
        public const string UserGetListSuccessful = "Successfully got list of users.";
        public const string VisitorGetListSuccessful = "Successfully got list of visitors.";
        public const string VisitorGetListError = "Error in retrieving list of visitors.";
        public const string VisitorGetSuccessful = "Successfully got visitor.";
        public const string VisitorGetError = "Error while getting visitor.";
        public const string VisitorDeleteSuccessfully = "Successfully deleted visitor.";
        public const string VisitorDeleteError = "Error while deleting visitor.";
        public const string VisitorCreateSuccessfully = "Created visitor successfully.";
        public const string VisitorCreateError = "Error while creating visitor.";
        public const string ErrorWrongRequest = "Error in request.";
        public const string ErrorInvalidRequest = "Invalid passing data.";
        public const string VisitorPresignedUrlGetSuccessfully = "Fetched value of presignedUrl successfully.";
        public const string VisitorPresignedUrlGetError = "Error while fetching value of presignedUrl.";
        public const string VisitorEmailError = "Error while sending email.";
        public const string VisitorUpdateSuccessfully = "Updated visitor successfully.";
        public const string VisitorUpdateError = "Error while updating visitor.";
        public const string UserIdEditError = "Error while editing user.";
        public const string UserIdEditSuccessfully = "Successfully edited user.";
        public const string UserDeleteSuccessfully = "Successfully deleted user.";
        public const string UserDeleteError = "Error while deleting user.";
        public const string UserDeleteAlreadySuccessfully = "User already deleted.";
        public const string UserIdGetError = "Error while getting user.";
        public const string UserIdGetSuccessfully = "Successfully got user.";
        public const string VisitorDetailsSuccessfully = "Successfully got visitor details.";
        public const string VisitorDetailsMonthwiseCount = "Successfully got cashier visitor count.";
        public const string VisitorDetailsError = "Error getting visitor details.";
        public const string VisitorIdNotFound = "Cannot find visitor.";
        public const string UserForgotPasswordEmailSuccessfully = "Email sent successfully.";
        public const string UserCreateError = "Error while creating user.";
        public const string VisitorChartCountSuccessfully = "Successfully fetch graph data.";
        public const string DataSuccessfully = "Successfully fetch data.";
        public const string EmailSentSuccessfully = "Email sent !";
        public const string HotelbookSuccessfully = "Hotel Book Successfully";
        public const string OTPSuccessfully = "OTP Send Successfully";
    }
}
