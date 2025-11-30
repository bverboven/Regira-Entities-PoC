<template>
  <section>
    <h1>404, Page not found</h1>
    <p>
      Page
      <router-link :to="url">{{ url }}</router-link>
      was not found.
    </p>
  </section>
</template>

<script setup>
defineProps({ url: { type: String, default: "/" } });
</script>
