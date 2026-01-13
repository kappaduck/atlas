export function responsiveExpand(): void {
  const size: string = getComputedStyle(document.body).getPropertyValue('--screen-md');
  const media: MediaQueryList = matchMedia(`(min-width: ${size})`);

  media.addEventListener('change', event => {
    if (!event.matches) {
      return;
    }

    const elements: NodeListOf<Element> = document.querySelectorAll('.expanded');
    const popover: HTMLElement | null = document.querySelector('#feedback');

    elements.forEach(toggleExpand);

    if (popover) {
      popover.hidePopover();
    }
  });
}

export function toggleMenu(): void {
  const header: HTMLElement | null = document.querySelector('header');
  const hamburger: Element | null = document.querySelector('.hamburger');

  if (!header || !hamburger) {
    return;
  }

  toggleExpand(header);
  toggleExpand(hamburger);
}

function toggleExpand(element: Element): void {
  const expanded: boolean = Boolean(element.getAttribute('aria-expanded'));

  element.classList.toggle('expanded');
  element.setAttribute('aria-expanded', String(!expanded));
}
