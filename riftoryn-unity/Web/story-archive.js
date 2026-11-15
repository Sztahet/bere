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
  const partsNav = document.getElementById("parts-navigation");
  const parts = ["I", "II", "III"];
  const elements = [["Ogień", "fire"], ["Lód", "ice"], ["Błyskawice", "lightning"], ["Natura", "nature"], ["Ziemia", "earth"], ["Światło", "light"], ["Ciemność", "darkness"]];
  const iconRoot = "riftoryn-unity/Assets/Resources/CodexIcons";
  let currentView;
  let storyLanguage = "pl";
  const sailaCovers = {
    "ciezar-krysztalu": {src: "riftoryn-unity/Assets/Resources/SailaStory/03.jpg", intro: "Przed zimą Saila utrzymuje w ruchu chłodnię, od której zależą zapasy przełęczy. Każde ładowanie kryształu kosztuje ją więcej sił."},
    "gdy-wieza-zamilkla": {src: "riftoryn-unity/Assets/Resources/SailaAwakening/05.jpg", intro: "Alarm na górskim szlaku zastaje Sailę w trakcie obchodu. Wieża strzela, a obsługa jest zbyt daleko, by zdążyć z pomocą."},
    "to-czego-nie-nauczyl-mnie-ruvan": {src: "riftoryn-unity/Assets/Resources/SailaFinale/04.jpg", intro: "Do Nivoren wracają pierwsze wozy z doliny. Gdy Ruvan przyjeżdża z wieściami o Igrzyskach, Saila musi zdecydować, ile chce wziąć na siebie."}
  };

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
    const availablePeople = storyRoster.filter(person => storyArchive.some(entry => entry.id === person.id));
    const upcomingPeople = storyRoster.filter(person => !storyArchive.some(entry => entry.id === person.id));

    function makeCard(person, available) {
      const card = available ? link(null, `#${person.id}`, "hero-card") : element("div", null, "hero-card unavailable");
      card.setAttribute("aria-label", `${person.name} — ${person.element}, ${person.role} — ${available ? "opowiadania dostępne" : "opowiadania w przygotowaniu"}`);
      if (available && person.id === hero.id) card.setAttribute("aria-current", "true");
      if (person.portrait) {
        const portrait = element("img", null, "hero-portrait");
        portrait.src = person.portrait; portrait.alt = ""; portrait.loading = "lazy";
        card.append(portrait);
      }
      const content = element("span", null, "hero-content");
      const affinity = element("span", null, "hero-affinity");
      const icon = elements.find(([name]) => name === person.element)?.[1];
      if (icon) {
        const image = element("img"); image.src = `${iconRoot}/elements/${icon}.png`; image.alt = "";
        affinity.append(image);
      }
      affinity.append(element("span", person.element));
      content.append(affinity, element("span", person.name, "hero-name"), element("span", person.role, "hero-role"));
      card.append(content);
      if (!available) card.append(element("span", "W przygotowaniu", "unavailable-label"));
      return card;
    }

    const headingBar = element("div", null, "heroes-heading");
    const headingText = element("div");
    headingText.append(element("span", "OPOWIEŚCI BOHATERÓW", "section-eyebrow"), element("h2", "Wybierz swoją historię"));
    headingBar.append(headingText, element("span", `${availablePeople.length} dostępnych historii`, "hero-count"));
    const grid = element("div", null, "hero-grid");
    grid.append(...availablePeople.map(person => makeCard(person, true)));
    const upcoming = element("details", null, "upcoming-heroes");
    const summary = element("summary");
    summary.append(element("span", "Bohaterowie w przygotowaniu"), element("span", `${upcomingPeople.length} postaci`, "hero-count"));
    const upcomingGrid = element("div", null, "hero-grid upcoming-grid");
    upcomingGrid.append(...upcomingPeople.map(person => makeCard(person, false)));
    upcoming.append(summary, upcomingGrid);
    heroes.replaceChildren(headingBar, grid, upcoming);

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
    if (!story.en) storyLanguage = "pl";
    const translated = storyLanguage === "en" && story.en ? story.en : story;
    const partLinks = hero.stories.map((part, partIndex) => {
      const entry = link(null, storyLink(hero, part), "parts-link");
      entry.append(element("span", `CZĘŚĆ ${parts[partIndex]}`, "parts-label"), element("span", part.title, "parts-title"));
      if (part === story) entry.setAttribute("aria-current", "page");
      return entry;
    });
    partsNav.replaceChildren(...partLinks);
    document.getElementById("back-to-collection").href = `#${hero.id}`;
    document.getElementById("reader-series").textContent = hero.series || "Archiwum Riftoryn";
    document.getElementById("reader-meta").textContent = [
      hero.name, `Część ${parts[index]}`, story.draft && "Wersja robocza"
    ].filter(Boolean).join(" · ");
    storyTitle.textContent = translated.title;
    const cover = hero.id === "saila" ? sailaCovers[story.id] : null;
    const coverImage = document.getElementById("reader-cover");
    coverImage.hidden = !cover;
    if (cover) coverImage.src = cover.src;
    else coverImage.removeAttribute("src");
    document.getElementById("reader-heading").classList.toggle("has-cover", Boolean(cover));
    const intro = document.getElementById("reader-intro");
    intro.textContent = cover?.intro || "";
    intro.hidden = !cover;
    document.getElementById("reader-start").href = `${storyLink(hero, story)}/rozdzial-1`;

    chapterNav.replaceChildren(element("strong", "Spis treści"));
    if (story.en) {
      const language = element("button", storyLanguage === "pl" ? "English" : "Polski");
      language.type = "button";
      language.addEventListener("click", () => { storyLanguage = storyLanguage === "pl" ? "en" : "pl"; renderStory(hero, story); });
      chapterNav.append(language);
    }
    storyBody.replaceChildren();
    translated.chapters.forEach((chapter, chapterIndex) => {
      const id = `rozdzial-${chapterIndex + 1}`;
      const section = element("section", null, "story-chapter");
      section.id = id;
      const headingWrap = element("div", null, "chapter-heading");
      headingWrap.append(element("span", `ROZDZIAŁ ${chapterIndex + 1}`, "chapter-number"));
      const heading = element("h2", chapter.title);
      heading.tabIndex = -1;
      headingWrap.append(heading);
      section.append(headingWrap);
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
