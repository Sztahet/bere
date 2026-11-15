(() => {
  "use strict";

  const archive = document.getElementById("archive");
  const reader = document.getElementById("reader");
  const heroes = document.getElementById("heroes");
  const collection = document.getElementById("collection");
  const storyTitle = document.getElementById("story-title");
  const chapterNav = document.getElementById("chapters");
  const storyBody = document.getElementById("story");
  const storyNav = document.getElementById("story-navigation");
  const parts = ["I", "II", "III"];
  const elements = [["Ogień", "fire"], ["Lód", "ice"], ["Błyskawice", "lightning"], ["Natura", "nature"], ["Ziemia", "earth"], ["Światło", "light"], ["Ciemność", "darkness"]];
  const roles = [["Tank", "tank"], ["Wsparcie", "support"], ["Specjalista", "specialist"], ["Przywoływacz", "summoner"]];
  const iconRoot = "riftoryn-unity/Assets/Resources/CodexIcons";
  let currentView;

  function element(tag, text, className) {
    const node = document.createElement(tag);
    if (text) node.textContent = text;
    if (className) node.className = className;
    return node;
  }

  function link(text, href, className) {
    const node = element("a", text, className);
    node.href = href;
    return node;
  }

  function storyLink(hero, story) {
    return `#${hero.id}/${story.id}`;
  }

  function renderCollection(hero) {
    const grid = element("div", null, "hero-grid");
    grid.append(element("div", null, "grid-corner"));
    elements.forEach(([name, icon]) => {
      const label = element("div", null, `grid-heading element-${icon}`);
      const image = element("img"); image.src = `${iconRoot}/elements/${icon}.png`; image.alt = "";
      label.append(image, element("span", name)); grid.append(label);
    });
    roles.forEach(([role, icon]) => {
      const label = element("div", null, "grid-role");
      const image = element("img"); image.src = `${iconRoot}/roles/${icon}.png`; image.alt = "";
      label.append(image, element("span", role)); grid.append(label);
      elements.forEach(([affinity]) => {
        const cell = element("div", null, "grid-cell");
        const matches = storyRoster.filter(person => person.role === role && person.element === affinity);
        if (!matches.length) cell.append(element("span", "Wkrótce", "empty-cell"));
        matches.forEach(person => {
          const available = storyArchive.find(entry => entry.id === person.id);
          const card = available ? link(null, `#${person.id}`, "hero-card") : element("div", null, "hero-card unavailable");
          card.setAttribute("aria-label", `${person.name} — ${available ? "opowiadania dostępne" : "opowiadania w przygotowaniu"}`);
          if (!available) card.title = `${person.name} — opowiadania w przygotowaniu`;
          if (available === hero) card.setAttribute("aria-current", "true");
          if (person.portrait) {
            const portrait = element("img"); portrait.src = person.portrait; portrait.alt = ""; portrait.loading = "lazy";
            card.append(portrait);
          }
          if (!available) card.append(element("span", "Wkrótce", "unavailable-label"));
          card.append(element("span", person.name, "hero-name"));
          cell.append(card);
        });
        grid.append(cell);
      });
    });
    heroes.replaceChildren(grid);

    const heading = element("h2", hero.series || `Opowiadania · ${hero.name}`);
    heading.id = "collection-title";
    heading.tabIndex = -1;
    const list = element("ol", null, "story-list");
    hero.stories.forEach((story, index) => {
      const item = element("li");
      const entry = link(null, storyLink(hero, story), "story-entry");
      entry.append(element("span", `Część ${parts[index]}`, "part"), element("span", story.title, "entry-title"));
      entry.append(element("span", "Czytaj →", "read-link"));
      item.append(entry);
      list.append(item);
    });
    collection.replaceChildren(heading, list);
  }

  function renderStory(hero, story) {
    const index = hero.stories.indexOf(story);
    document.getElementById("back-to-collection").href = `#${hero.id}`;
    document.getElementById("reader-series").textContent = hero.series || "Archiwum Riftoryn";
    document.getElementById("reader-meta").textContent = [
      hero.name, `Część ${parts[index]}`, story.draft && "Wersja robocza"
    ].filter(Boolean).join(" · ");
    storyTitle.textContent = story.title;

    chapterNav.replaceChildren(element("strong", "Spis treści"));
    storyBody.replaceChildren();
    story.chapters.forEach((chapter, chapterIndex) => {
      const id = `rozdzial-${chapterIndex + 1}`;
      const section = element("section", null, "story-chapter");
      section.id = id;
      const heading = element("h2", chapter.title);
      heading.tabIndex = -1;
      section.append(heading);
      for (const block of chapter.blocks) {
        if (block.src) {
          const figure = element("figure");
          const image = element("img");
          image.src = block.src;
          image.alt = block.alt || "Ilustracja do opowiadania";
          image.loading = "lazy";
          image.width = block.width ?? 1536;
          image.height = block.height ?? 1024;
          figure.append(image);
          section.append(figure);
        } else if (typeof block.text === "string") {
          section.append(element("p", block.text));
        }
      }
      chapterNav.append(link(chapter.title, `${storyLink(hero, story)}/${id}`));
      storyBody.append(section);
    });

    const previous = hero.stories[index - 1];
    const next = hero.stories[index + 1];
    storyNav.replaceChildren(
      link(previous ? `← ${previous.title}` : "← Wszystkie części", previous ? storyLink(hero, previous) : `#${hero.id}`),
      link(next ? `${next.title} →` : "Wróć do opowiadań →", next ? storyLink(hero, next) : `#${hero.id}`)
    );
  }

  function render(focus = false) {
    const [heroId, storyId, chapterId] = location.hash.slice(1).split("/");
    const hero = storyArchive.find(person => person.id === heroId) || storyArchive[0];
    const story = hero.stories.find(entry => entry.id === storyId);
    const view = story ? storyLink(hero, story) : `#${hero.id}`;
    const changed = view !== currentView;
    const wasReading = !reader.hidden;
    if (changed) {
      currentView = view;
      reader.hidden = !story;
      archive.hidden = Boolean(story);
      document.title = story ? `${story.title} — Archiwum Riftoryn` : "Archiwum Riftoryn";
      if (story) renderStory(hero, story);
      else renderCollection(hero);
    }

    const chapter = story && /^rozdzial-\d+$/.test(chapterId || "") && document.getElementById(chapterId);
    if (chapter) {
      if (focus) chapter.querySelector("h2").focus({ preventScroll: true });
      chapter.scrollIntoView();
    } else if (focus && changed && !story && !wasReading) {
      collection.querySelector("h2").focus({ preventScroll: true });
      collection.scrollIntoView({ block: "nearest" });
    } else if (changed || story) {
      const active = story ? storyTitle : heroes.querySelector('[aria-current="true"]');
      if (focus) active.focus({ preventScroll: true });
      window.scrollTo(0, 0);
    }
  }

  window.addEventListener("hashchange", () => render(true));
  render();
})();
