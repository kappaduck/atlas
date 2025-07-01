export function responsiveExpand(): void {
  const size = getComputedStyle(document.body).getPropertyValue('--screen-md');
  const media = matchMedia(`(min-width: ${size})`);

  media.addEventListener('change', event => {
    if (!event.matches) {
      return;
    }

    const elements = document.querySelectorAll('.expanded');
    const popover: HTMLElement | null = document.querySelector('#feedback');

    elements.forEach(toggleExpand);

    if (popover) {
      popover.hidePopover();
    }
  });
}

export function toggleMenu(): void {
  const header = document.querySelector('header');
  const hamburger = document.querySelector('.hamburger');

  if (!header || !hamburger) {
    return;
  }

  toggleExpand(header);
  toggleExpand(hamburger);
}

function toggleExpand(element: Element): void {
  const expanded = Boolean(element.getAttribute('aria-expanded'));

  element.classList.toggle('expanded');
  element.setAttribute('aria-expanded', String(!expanded));
}
