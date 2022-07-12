export default class RolePermissions {
  private static readonly Admin: { [key: string]: boolean } = {
    Admin: false,
    User: true,
    Guest: true,
  }

  private static readonly User: { [key: string]: boolean } = {
    Admin: false,
    User: true,
    Guest: true,
  }

  private static readonly Guest: { [key: string]: boolean } = {
    Admin: false,
    User: false,
    Guest: true,
  }

  public static readonly permissions: { [key: string]: any } = {
    'Admin': this.Admin,
    'User': this.User,
    'Guest': this.Guest,
  };
}