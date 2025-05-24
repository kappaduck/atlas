export function changeTheme(theme: string): void {
  const app = document.querySelector('#app');

  app?.setAttribute('data-theme', theme);
}
