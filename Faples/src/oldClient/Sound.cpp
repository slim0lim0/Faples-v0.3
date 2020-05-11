#include "sound.hpp"
#include "map.hpp"
#include <FaplesFpx/audio.hpp>
#include <FaplesFpx/fpx.hpp>
#include <mpg123.h>
#include <portaudio.h>
#include <memory>
#include <locale>
#include <iostream>
#include <thread>
#include <chrono>
#include <atomic>

namespace fpl {
	namespace sound {
		node n = {};
		struct stream_t {
			PaStream * stream = nullptr;
			mpg123_handle * handle = nullptr;
			audio a;
			int channels = 0;
			std::atomic_bool stop;
		};
		stream_t * active = nullptr;
		void init() {
			if (mpg123_init() != MPG123_OK) throw std::runtime_error("Failed to initialize mpg123");
			if (Pa_Initialize() != paNoError) throw std::runtime_error("Failed to initialize PortAudio");
		}

		void unload() {
			mpg123_exit();
			Pa_Terminate();
		}

		int callback(const void *, void * output, unsigned long frames, PaStreamCallbackTimeInfo const *,
			PaStreamCallbackFlags, void * user) {
			auto stream = reinterpret_cast<stream_t *>(user);
			auto todo = static_cast<size_t>(frames * 2 * stream->channels);
			auto buf = reinterpret_cast<unsigned char *>(output);
			while (todo) {
				size_t done;
				auto err = mpg123_read(stream->handle, buf, todo, &done);
				todo -= done;
				buf += done;
				if (err == MPG123_NEED_MORE) {
					mpg123_feed(stream->handle,
						reinterpret_cast<unsigned char const *>(stream->a.data()) + 82,
						stream->a.length() - 82);
				}
			}
			if (stream->stop == true) {
				stream->stop = false;
				auto a = reinterpret_cast<int16_t *>(output);
				auto s = frames * stream->channels;
				for (auto i = 0ul; i < s; ++i)
					a[i] = static_cast<int16_t>(static_cast<double>(s - i) / s * a[i]);
				std::thread([](stream_t * stream) {
					Pa_StopStream(stream->stream);
					Pa_CloseStream(stream->stream);
					mpg123_close(stream->handle);
					delete stream;
				}, stream).detach();
				return paComplete;
			}
			return paContinue;
		}

		void play(node nn) {
			if (n == nn) return;
			n = nn;
			if (active) {
				active->stop = true;
				active = nullptr;
			}
			audio a = n;
			if (!a) {
				//log << "Map does not contain valid bgm" << std::endl;
				return;
			}
			active = new stream_t();
			active->a = n;
			active->stop = false;
			active->handle = mpg123_new(nullptr, nullptr);
			if (!active->handle) throw std::runtime_error("Failed to open mpg123 handle");
			mpg123_open_feed(active->handle);
			mpg123_feed(active->handle, reinterpret_cast<unsigned char const *>(a.data()) + 82,
				a.length() - 82);
			long rate;
			int encoding;
			if (mpg123_getformat(active->handle, &rate, &active->channels, &encoding) != MPG123_OK)
				throw std::runtime_error("Failed to get format of music");
			auto err = Pa_OpenDefaultStream(&active->stream, 0, active->channels, paInt16, rate,
				static_cast<unsigned long>(mpg123_outblock(active->handle)),
				callback, active);
			if (err != paNoError) {
				//log << "Failed to open PortAudio stream: " << err << std::endl;
				delete active;
				active = nullptr;
				return;
			}
			Pa_StartStream(active->stream);
		}

		void play() {
			std::string bgm = map::current.GetNode(map::current.root(), "bgmusic");
			/*if (islower(bgm[0])) bgm[0] = std::toupper(bgm[0], std::locale::classic());
			while (bgm.find(' ') != bgm.npos) bgm.erase(bgm.find(' '), 1);*/
			//auto p = bgm.find('/');
			auto sn = fpx::fAudio.GetNode(map::current.root(), "bgm/00/" + bgm);
			//if (!sn) log << "Failed to find bgm " << bgm << " for map " << map::current_name << std::endl;
			play(sn);
		}
	}
}