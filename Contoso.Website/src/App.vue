<script setup lang="ts">
import { Feedback, LoadingContainer } from "@/regira_modules/vue/ui"
import { AppStatus } from "@/regira_modules/vue/app"
import Header from "@/components/layout/TheHeader.vue"
import Footer from "@/components/layout/TheFooter.vue"
import Main from "@/components/layout/Main.vue"
import Offline from "@/components/layout/Offline.vue"
import Debug from "@/components/layout/Debug.vue"
</script>

<template>
  <div class="page">
    <Offline />

    <header class="container-fluid bg-light">
      <Header />
    </header>

    <section class="container-fluid feedback-container position-relative overflow-hidden">
      <Feedback :feedback="$feedback" :enable-error-popup="true" />
      <Debug />
    </section>

    <main class="container-fluid">
      <LoadingContainer :isLoading="$appStatus != AppStatus.Ready && (!$auth.enabled || $auth.isAuthenticated)">
        <Main />
      </LoadingContainer>
    </main>

    <footer class="container-fluid bg-light">
      <Footer />
    </footer>
  </div>
</template>
